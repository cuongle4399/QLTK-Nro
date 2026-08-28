using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QLTK_Nro_Pro.Presenter
{
    public class ServerItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ServerItem() { }

        public ServerItem(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => $"{Id}|{Name}";
    }

    public static class ServerManager
    {
        private static readonly List<ServerItem> _servers = new List<ServerItem>();
        private static readonly object _lock = new object();

        public static IReadOnlyList<ServerItem> Servers
        {
            get
            {
                lock (_lock)
                {
                    return _servers.ToList();
                }
            }
        }

        /// <summary>
        /// Danh sách server mặc định ban đầu
        /// </summary>
        public static List<ServerItem> GetDefaultServers()
        {
            return new List<ServerItem>
            {
                new ServerItem(1, "1"),
                new ServerItem(2, "2"),
                new ServerItem(3, "3"),
                new ServerItem(4, "4"),
                new ServerItem(5, "5"),
                new ServerItem(6, "6"),
                new ServerItem(7, "7"),
                new ServerItem(8, "8"),
                new ServerItem(9, "9"),
                new ServerItem(10, "10"),
                new ServerItem(11, "11"),
                new ServerItem(12, "12"),
                new ServerItem(13, "Võ đài liên vũ trụ [13]"),
                new ServerItem(14, "Universe1 (14)"),
                new ServerItem(15, "Naga [15]"),
                new ServerItem(16, "Super 1 [16]"),
                new ServerItem(17, "Super 2 [17]"),
                new ServerItem(18, "13 [18]"),
                new ServerItem(19, "VIP 2 [19]"),
                new ServerItem(20, "14 [20]"),
                new ServerItem(21, "Super 3 [21]"),
                new ServerItem(22, "Vũ Trụ 15 [22]")
            };
        }

        public static void Init()
        {
            LoadServers();
        }

        public static void LoadServers()
        {
            lock (_lock)
            {
                try
                {
                    string path = AppConstants.PathServers;
                    string dir = Path.GetDirectoryName(path);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    if (!File.Exists(path))
                    {
                        _servers.Clear();
                        _servers.AddRange(GetDefaultServers());
                        SaveServersInternal();
                        return;
                    }

                    string[] lines = File.ReadAllLines(path, Encoding.UTF8);
                    List<ServerItem> loaded = new List<ServerItem>();

                    foreach (string rawLine in lines)
                    {
                        if (string.IsNullOrWhiteSpace(rawLine))
                            continue;

                        string line = rawLine.Trim();
                        if (line.StartsWith("#") || line.StartsWith("//"))
                            continue;

                        int splitIndex = line.IndexOf('|');
                        if (splitIndex > 0)
                        {
                            string idStr = line.Substring(0, splitIndex).Trim();
                            string nameStr = line.Substring(splitIndex + 1).Trim();

                            if (int.TryParse(idStr, out int id) && !string.IsNullOrEmpty(nameStr))
                            {
                                loaded.Add(new ServerItem(id, nameStr));
                            }
                        }
                    }

                    if (loaded.Count > 0)
                    {
                        _servers.Clear();
                        _servers.AddRange(loaded);
                    }
                    else
                    {
                        _servers.Clear();
                        _servers.AddRange(GetDefaultServers());
                        SaveServersInternal();
                    }
                }
                catch
                {
                    if (_servers.Count == 0)
                    {
                        _servers.AddRange(GetDefaultServers());
                    }
                }
            }
        }

        public static void SaveServers(List<ServerItem> serverList)
        {
            lock (_lock)
            {
                _servers.Clear();
                _servers.AddRange(serverList);
                SaveServersInternal();
            }
        }

        private static void SaveServersInternal()
        {
            try
            {
                string path = AppConstants.PathServers;
                string dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                List<string> lines = new List<string>();
                foreach (var s in _servers)
                {
                    lines.Add($"{s.Id}|{s.Name}");
                }
                File.WriteAllLines(path, lines, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ServerManager] Save error: {ex.Message}");
            }
        }

        public static string GetServerName(int id)
        {
            lock (_lock)
            {
                if (_servers.Count == 0)
                    LoadServers();

                var item = _servers.FirstOrDefault(s => s.Id == id);
                if (item != null)
                    return item.Name;

                return id.ToString();
            }
        }

        public static int GetServerId(string x)
        {
            if (string.IsNullOrWhiteSpace(x) || x.StartsWith("+ Thêm") || x.Contains("Thêm Server") || x.Contains("Quản lý"))
                return 0;

            string trimmed = x.Trim();

            lock (_lock)
            {
                if (_servers.Count == 0)
                    LoadServers();

                // 1. Exact match by Name
                var item = _servers.FirstOrDefault(s => s.Name.Equals(trimmed, StringComparison.OrdinalIgnoreCase));
                if (item != null)
                    return item.Id;

                // 2. Format like "Name [22]" or "[22]"
                Match match = Regex.Match(trimmed, @"\[(\d+)\]");
                if (match.Success && int.TryParse(match.Groups[1].Value, out int extractedId))
                {
                    return extractedId;
                }

                // 3. Format like "Universe1 (14)"
                Match matchParen = Regex.Match(trimmed, @"\((\d+)\)");
                if (matchParen.Success && int.TryParse(matchParen.Groups[1].Value, out int extractedIdParen))
                {
                    return extractedIdParen;
                }

                // 4. Directly numeric
                if (int.TryParse(trimmed, out int directId))
                {
                    return directId;
                }
            }

            return 0;
        }

        /// <summary>
        /// Tự động thêm hoặc đảm bảo server tồn tại từ chuỗi nhập (hỗ trợ "id|tên", "Tên [id]", "id")
        /// </summary>
        public static ServerItem AddOrEnsureServer(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            string trimmed = input.Trim();
            int id = 0;
            string name = string.Empty;

            // 1. Format: "id|name" (vd: 23|Vũ Trụ 16 [23])
            if (trimmed.Contains('|'))
            {
                string[] parts = trimmed.Split('|');
                if (int.TryParse(parts[0].Trim(), out int parsedId) && parsedId > 0)
                {
                    id = parsedId;
                    name = parts[1].Trim();
                    if (string.IsNullOrEmpty(name)) name = id.ToString();
                }
            }
            // 2. Format có "[id]" (vd: "Vũ Trụ 16 [23]")
            else
            {
                Match match = Regex.Match(trimmed, @"\[(\d+)\]");
                if (match.Success && int.TryParse(match.Groups[1].Value, out int extractedId))
                {
                    id = extractedId;
                    name = trimmed;
                }
                else
                {
                    Match matchParen = Regex.Match(trimmed, @"\((\d+)\)");
                    if (matchParen.Success && int.TryParse(matchParen.Groups[1].Value, out int extractedParenId))
                    {
                        id = extractedParenId;
                        name = trimmed;
                    }
                    else if (int.TryParse(trimmed, out int directId) && directId > 0)
                    {
                        id = directId;
                        name = id.ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            if (id <= 0) return null;

            lock (_lock)
            {
                if (_servers.Count == 0)
                    LoadServers();

                var existing = _servers.FirstOrDefault(s => s.Id == id);
                if (existing == null)
                {
                    var newItem = new ServerItem(id, string.IsNullOrEmpty(name) ? id.ToString() : name);
                    _servers.Add(newItem);
                    SaveServersInternal();
                    return newItem;
                }
                else
                {
                    if (!string.IsNullOrEmpty(name) && !existing.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        if (trimmed.Contains('|') || trimmed.Contains('[') || trimmed.Contains('('))
                        {
                            existing.Name = name;
                            SaveServersInternal();
                        }
                    }
                    return existing;
                }
            }
        }

        public const string ADD_NEW_SERVER_ITEM = "+ Thêm Server";

        public static void PopulateComboBox(ComboBox comboBox)
        {
            if (comboBox == null) return;

            string previousText = comboBox.Text;
            int previousIndex = comboBox.SelectedIndex;

            lock (_lock)
            {
                if (_servers.Count == 0)
                    LoadServers();

                comboBox.BeginUpdate();
                comboBox.Items.Clear();

                foreach (var server in _servers)
                {
                    comboBox.Items.Add(server.Name);
                }

                comboBox.Items.Add(ADD_NEW_SERVER_ITEM);

                comboBox.EndUpdate();
            }

            if (!string.IsNullOrEmpty(previousText) && previousText != ADD_NEW_SERVER_ITEM)
            {
                int index = comboBox.FindStringExact(previousText);
                if (index >= 0)
                    comboBox.SelectedIndex = index;
                else
                    comboBox.Text = previousText;
            }
            else if (previousIndex >= 0 && previousIndex < _servers.Count)
            {
                comboBox.SelectedIndex = previousIndex;
            }
            else if (_servers.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }
    }
}
