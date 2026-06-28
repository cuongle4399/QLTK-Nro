using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ModCak.main.Mod
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error,
        Socket,
        System
    }

    public class LogEntry
    {
        public DateTime Timestamp { get; }
        public string Message { get; }
        public LogLevel Level { get; }

        public LogEntry(string message, LogLevel level)
        {
            Timestamp = DateTime.Now;
            Message = message;
            Level = level;
        }

        public string GetStrippedMessage()
        {
            return Regex.Replace(Message, "<.*?>", string.Empty);
        }
    }

    public static class WinConsole
    {
        #region Windows Win32 API Imports

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SetConsoleTitle(string lpConsoleTitle);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool WriteConsoleW(
            IntPtr hConsoleOutput,
            string lpBuffer,
            uint nNumberOfCharsToWrite,
            out uint lpNumberOfCharsWritten,
            IntPtr lpReserved
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool ReadConsoleW(
            IntPtr hConsoleInput,
            [Out] StringBuilder lpBuffer,
            uint nNumberOfCharsToRead,
            out uint lpNumberOfCharsRead,
            IntPtr pInputControl
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, ushort wAttributes);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetNumberOfConsoleInputEvents(IntPtr hConsoleInput, out uint lpcNumberOfEvents);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool ReadConsoleInputW(
            IntPtr hConsoleInput,
            [Out] INPUT_RECORD[] lpBuffer,
            uint nLength,
            out uint lpNumberOfEventsRead
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleScreenBufferInfo(IntPtr hConsoleOutput, out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FillConsoleOutputCharacterA(IntPtr hConsoleOutput, char cCharacter, uint nLength, COORD dwWriteCoord, out uint lpNumberOfCharsWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FillConsoleOutputAttribute(IntPtr hConsoleOutput, ushort wAttribute, uint nLength, COORD dwWriteCoord, out uint lpNumberOfAttrsWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCursorPosition(IntPtr hConsoleOutput, COORD dwCursorPosition);

        #endregion

        #region Win32 Constants and Structs

        private const int STD_INPUT_HANDLE = -10;
        private const int STD_OUTPUT_HANDLE = -11;

        [StructLayout(LayoutKind.Sequential)]
        private struct COORD
        {
            public short X;
            public short Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CONSOLE_SCREEN_BUFFER_INFO
        {
            public COORD dwSize;
            public COORD dwCursorPosition;
            public ushort wAttributes;
            public SMALL_RECT srWindow;
            public COORD dwMaximumWindowSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SMALL_RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct INPUT_RECORD
        {
            [FieldOffset(0)]
            public ushort EventType;
            [FieldOffset(4)]
            public KEY_EVENT_RECORD KeyEvent;
        }

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
        private struct KEY_EVENT_RECORD
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.Bool)]
            public bool bKeyDown;
            [FieldOffset(4)]
            public ushort wRepeatCount;
            [FieldOffset(6)]
            public ushort wVirtualKeyCode;
            [FieldOffset(8)]
            public ushort wVirtualScanCode;
            [FieldOffset(10)]
            public char UnicodeChar;
            [FieldOffset(12)]
            public uint dwControlKeyState;
        }

        #endregion

        #region Fields

        private static IntPtr _outHandle = IntPtr.Zero;
        private static IntPtr _inHandle = IntPtr.Zero;
        private static bool _initialized;
        private static readonly List<LogEntry> LogBuffer = new List<LogEntry>();
        private static readonly object LockObj = new object();
        private const int MaxBufferSize = 1000;

        // Interactive States
        private static bool _isPaused;
        private static string _currentFilter = string.Empty;
        private static readonly HashSet<LogLevel> ActiveLevels = new HashSet<LogLevel>
        {
            LogLevel.Info, LogLevel.Warning, LogLevel.Error, LogLevel.Socket, LogLevel.System
        };

        #endregion

        public static bool IsInitialized => _initialized;

        public static bool Init()
        {
            if (_initialized)
                return true;

            if (!AllocConsole())
                return false;

            SetConsoleTitle("Debug Client Nro - Developer Console");

            _outHandle = GetStdHandle(STD_OUTPUT_HANDLE);
            _inHandle = GetStdHandle(STD_INPUT_HANDLE);

            if (_outHandle == IntPtr.Zero || _outHandle == new IntPtr(-1))
                return false;

            _initialized = true;

            PrintHelpHeader();
            StartInputLoop();

            return true;
        }

        public static void WriteLine(string text)
        {
            WriteLine(text, LogLevel.Info);
        }

        public static void WriteLine(string text, LogLevel level)
        {
            if (!_initialized)
                return;

            var entry = new LogEntry(text, level);

            lock (LockObj)
            {
                if (LogBuffer.Count >= MaxBufferSize)
                {
                    LogBuffer.RemoveAt(0);
                }
                LogBuffer.Add(entry);

                if (_isPaused)
                    return;

                if (!ActiveLevels.Contains(level))
                    return;

                if (!string.IsNullOrEmpty(_currentFilter) && 
                    !entry.GetStrippedMessage().ToLower().Contains(_currentFilter.ToLower()))
                {
                    return;
                }

                PrintLog(entry);
            }
        }

        #region Console Printing Core

        private static void WriteRaw(string text)
        {
            if (!_initialized || _outHandle == IntPtr.Zero)
                return;

            WriteConsoleW(
                _outHandle,
                text,
                (uint)text.Length,
                out _,
                IntPtr.Zero
            );
        }

        private static void SetColor(ConsoleColor color)
        {
            if (_outHandle == IntPtr.Zero) return;
            SetConsoleTextAttribute(_outHandle, (ushort)color);
        }

        private static void ClearConsole()
        {
            if (_outHandle == IntPtr.Zero) return;

            if (GetConsoleScreenBufferInfo(_outHandle, out CONSOLE_SCREEN_BUFFER_INFO csbi))
            {
                uint numChars = (uint)(csbi.dwSize.X * csbi.dwSize.Y);
                COORD home = new COORD { X = 0, Y = 0 };

                FillConsoleOutputCharacterA(_outHandle, ' ', numChars, home, out _);
                FillConsoleOutputAttribute(_outHandle, csbi.wAttributes, numChars, home, out _);
                SetConsoleCursorPosition(_outHandle, home);
            }
        }

        private static void PrintLog(LogEntry entry)
        {
            string timeStr = entry.Timestamp.ToString("HH:mm:ss");
            string levelStr = entry.Level.ToString().ToUpper().PadRight(7);

            // Print prefix
            SetColor(ConsoleColor.DarkGray);
            WriteRaw($"[{timeStr}] ");

            SetColor(GetColorForLogLevel(entry.Level));
            WriteRaw($"[{levelStr}] ");

            // Print formatted body text
            PrintFormattedText(entry.Message, entry.Level);
        }

        private static void PrintFormattedText(string text, LogLevel defaultLevel)
        {
            ConsoleColor defaultColor = GetColorForLogLevel(defaultLevel);
            SetColor(defaultColor);

            int index = 0;
            while (index < text.Length)
            {
                int tagStart = text.IndexOf('<', index);
                if (tagStart == -1)
                {
                    WriteRaw(text.Substring(index));
                    break;
                }

                if (tagStart > index)
                {
                    WriteRaw(text.Substring(index, tagStart - index));
                }

                int tagEnd = text.IndexOf('>', tagStart);
                if (tagEnd == -1)
                {
                    WriteRaw(text.Substring(tagStart));
                    break;
                }

                string tag = text.Substring(tagStart, tagEnd - tagStart + 1);
                index = tagEnd + 1;

                if (tag.StartsWith("<color="))
                {
                    string hexColor = tag.Substring(7, tag.Length - 8);
                    SetColor(ParseHexColor(hexColor, defaultColor));
                }
                else if (tag == "</color>")
                {
                    SetColor(defaultColor);
                }
            }
            WriteRaw("\n");
            SetColor(ConsoleColor.Gray);
        }

        private static ConsoleColor GetColorForLogLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Warning: return ConsoleColor.Yellow;
                case LogLevel.Error: return ConsoleColor.Red;
                case LogLevel.Socket: return ConsoleColor.Cyan;
                case LogLevel.System: return ConsoleColor.Green;
                default: return ConsoleColor.Gray;
            }
        }

        private static ConsoleColor ParseHexColor(string hex, ConsoleColor defaultColor)
        {
            hex = hex.Trim().ToLower().Replace("#", "");
            if (hex.StartsWith("ff0000")) return ConsoleColor.Red;
            if (hex.StartsWith("ffff00")) return ConsoleColor.Yellow;
            if (hex.StartsWith("00ff00")) return ConsoleColor.Green;
            if (hex.StartsWith("00ffff")) return ConsoleColor.Cyan;
            if (hex.StartsWith("ff00ff")) return ConsoleColor.Magenta;
            if (hex.StartsWith("ffffff")) return ConsoleColor.White;
            if (hex.StartsWith("808080")) return ConsoleColor.DarkGray;
            return defaultColor;
        }

        #endregion

        #region Keyboard Interaction Loop

        private static void StartInputLoop()
        {
            var thread = new Thread(InputLoop)
            {
                IsBackground = true
            };
            thread.Start();
        }

        private static void InputLoop()
        {
            try
            {
                INPUT_RECORD[] recordBuffer = new INPUT_RECORD[1];
                while (_initialized)
                {
                    if (GetNumberOfConsoleInputEvents(_inHandle, out uint numEvents) && numEvents > 0)
                    {
                        if (ReadConsoleInputW(_inHandle, recordBuffer, 1, out uint read) && read > 0)
                        {
                            var record = recordBuffer[0];
                            if (record.EventType == 0x0001) // KEY_EVENT
                            {
                                if (record.KeyEvent.bKeyDown)
                                {
                                    HandleWin32Key(record.KeyEvent);
                                }
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
            }
            catch
            {
                // Ignore background console failures
            }
        }

        private static void HandleWin32Key(KEY_EVENT_RECORD keyEvent)
        {
            lock (LockObj)
            {
                switch (keyEvent.wVirtualKeyCode)
                {
                    case 0x48: // H
                        PrintHelpHeader();
                        break;

                    case 0x43: // C
                        ClearConsole();
                        WriteRaw("--- CONSOLE CLEARED ---\n");
                        break;

                    case 0x50: // P
                        _isPaused = !_isPaused;
                        SetColor(_isPaused ? ConsoleColor.Yellow : ConsoleColor.Green);
                        WriteRaw(_isPaused ? "\n--- LOG STREAM PAUSED ---\n" : "\n--- LOG STREAM RESUMED ---\n");
                        SetColor(ConsoleColor.Gray);
                        break;

                    case 0x46: // F
                        PromptForFilter();
                        break;

                    case 0x4c: // L
                        _currentFilter = string.Empty;
                        SetColor(ConsoleColor.Green);
                        WriteRaw("\n--- FILTERS CLEARED. REPRINTING BUFFER ---\n");
                        SetColor(ConsoleColor.Gray);
                        ReprintBuffer();
                        break;

                    case 0x31: // 1
                        ToggleLevel(LogLevel.Info, "INFO");
                        break;
                    case 0x32: // 2
                        ToggleLevel(LogLevel.Warning, "WARNING");
                        break;
                    case 0x33: // 3
                        ToggleLevel(LogLevel.Error, "ERROR");
                        break;
                    case 0x34: // 4
                        ToggleLevel(LogLevel.Socket, "SOCKET");
                        break;
                    case 0x35: // 5
                        ToggleLevel(LogLevel.System, "SYSTEM");
                        break;
                }
            }
        }

        private static void ToggleLevel(LogLevel level, string name)
        {
            if (ActiveLevels.Contains(level))
            {
                ActiveLevels.Remove(level);
                SetColor(ConsoleColor.Red);
                WriteRaw($"\n[SYSTEM] HIDE Level {name}\n");
            }
            else
            {
                ActiveLevels.Add(level);
                SetColor(ConsoleColor.Green);
                WriteRaw($"\n[SYSTEM] SHOW Level {name}\n");
            }
            SetColor(ConsoleColor.Gray);
            ReprintBuffer();
        }

        private static void PromptForFilter()
        {
            SetColor(ConsoleColor.Magenta);
            WriteRaw("\n[SEARCH] Nhập từ khóa tìm kiếm (bấm Enter để xác nhận): ");
            SetColor(ConsoleColor.Gray);

            StringBuilder sb = new StringBuilder(256);
            if (ReadConsoleW(_inHandle, sb, 256, out uint read, IntPtr.Zero))
            {
                string query = sb.ToString().Substring(0, (int)read).Replace("\r", "").Replace("\n", "");
                _currentFilter = query;

                SetColor(ConsoleColor.Green);
                WriteRaw($"\n--- FILTERING BY: '{_currentFilter}' ---\n");
                SetColor(ConsoleColor.Gray);
                ReprintBuffer();
            }
        }

        private static void ReprintBuffer()
        {
            ClearConsole();
            SetColor(ConsoleColor.Cyan);
            WriteRaw($"=== LOG REPRINT (Filter: '{_currentFilter}', Paused: {_isPaused}) ===\n");
            SetColor(ConsoleColor.Gray);

            foreach (var entry in LogBuffer)
            {
                if (!ActiveLevels.Contains(entry.Level))
                    continue;

                if (!string.IsNullOrEmpty(_currentFilter) && 
                    !entry.GetStrippedMessage().ToLower().Contains(_currentFilter.ToLower()))
                {
                    continue;
                }

                PrintLog(entry);
            }
        }

        private static void PrintHelpHeader()
        {
            SetColor(ConsoleColor.Cyan);
            WriteRaw("=========================================================================\n");
            WriteRaw("             DEBUG DEVELOPER CONSOLE - HOTKEYS HƯỚNG DẪN                 \n");
            WriteRaw("=========================================================================\n");
            SetColor(ConsoleColor.White);
            WriteRaw("  [H] - Hiển thị menu hướng dẫn này\n");
            WriteRaw("  [C] - Xóa sạch màn hình Console (Clear Screen)\n");
            WriteRaw("  [P] - Tạm dừng / Tiếp tục nhận log (Pause / Resume)\n");
            WriteRaw("  [F] - Lọc log theo từ khóa tìm kiếm (Search / Filter)\n");
            WriteRaw("  [L] - Reset bộ lọc (Hiển thị lại toàn bộ log trong bộ nhớ đệm)\n");
            WriteRaw("  [1] - Bật/Tắt hiển thị log level: [INFO]\n");
            WriteRaw("  [2] - Bật/Tắt hiển thị log level: [WARNING]\n");
            WriteRaw("  [3] - Bật/Tắt hiển thị log level: [ERROR]\n");
            WriteRaw("  [4] - Bật/Tắt hiển thị log level: [SOCKET]\n");
            WriteRaw("  [5] - Bật/Tắt hiển thị log level: [SYSTEM]\n");
            SetColor(ConsoleColor.Cyan);
            WriteRaw("=========================================================================\n");
            SetColor(ConsoleColor.Gray);
        }

        #endregion
    }
}
