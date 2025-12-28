using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using File = System.IO.File;

namespace QLTK_Nro_Pro.Presenter
{
    public class CaptchaApiManager
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly Label _lblAPICapcha;
        private readonly PictureBox _pBAPI;
        private readonly ComboBox _cbbServerAPI;
        private readonly TextBox _txtServerAPI;
        private bool _isLoading = false;

        public CaptchaApiManager(Label lblAPICapcha, PictureBox pBAPI, ComboBox cbbServerAPI, TextBox txtServerAPI)
        {
            _lblAPICapcha = lblAPICapcha;
            _pBAPI = pBAPI;
            _cbbServerAPI = cbbServerAPI;
            _txtServerAPI = txtServerAPI;
        }

        /// <summary>
        /// Hiển thị hình ảnh Captcha từ base64
        /// </summary>
        public void ShowImage(string base64)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                using (var ms = new System.IO.MemoryStream(bytes))
                {
                    _pBAPI.Image = System.Drawing.Image.FromStream(ms);
                }
            }
            catch
            {
                _lblAPICapcha.Text = "Ảnh Capcha lỗi!";
                _lblAPICapcha.ForeColor = System.Drawing.Color.Red;
            }
        }

        /// <summary>
        /// Test captcha - gửi yêu cầu tới server API
        /// </summary>
        public async Task TestCaptcha(string apiKey, string base64Image)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_txtServerAPI.Text))
                {
                    _lblAPICapcha.ForeColor = System.Drawing.Color.Red;
                    _lblAPICapcha.Text = "Vui lòng nhập địa chỉ Server API";
                    return;
                }

                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    _lblAPICapcha.ForeColor = System.Drawing.Color.Red;
                    _lblAPICapcha.Text = "Vui lòng nhập API Key";
                    return;
                }

                string url = $"{_txtServerAPI.Text}{apiKey}";
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    _lblAPICapcha.ForeColor = System.Drawing.Color.Red;
                    _lblAPICapcha.Text = "Địa chỉ API không hợp lệ";
                    return;
                }

                _lblAPICapcha.ForeColor = System.Drawing.Color.Gray;
                _lblAPICapcha.Text = "📤 Đang gửi yêu cầu...";

                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["image"] = base64Image
                });

                var res = await _httpClient.PostAsync(url, content);
                string responseText = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                {
                    _lblAPICapcha.ForeColor = System.Drawing.Color.Red;
                    _lblAPICapcha.Text =
                        $"{ToVietnameseError((int)res.StatusCode, null)}\n\n" +
                        $"HTTP {(int)res.StatusCode} {res.StatusCode}\n{responseText}";
                    return;
                }

                try
                {
                    using var doc = JsonDocument.Parse(responseText);
                    var root = doc.RootElement;

                    int status = root.TryGetProperty("status", out var st)
                        ? st.GetInt32()
                        : -1;

                    string captcha = root.TryGetProperty("captcha", out var cap)
                        ? cap.GetString() ?? ""
                        : "";

                    string message = root.TryGetProperty("message", out var msg)
                        ? msg.GetString() ?? ""
                        : "";

                    if (status == 0)
                    {
                        _lblAPICapcha.ForeColor = System.Drawing.Color.Green;
                        _lblAPICapcha.Text =
                            $"✅ Nhận diện thành công\n" +
                            $"🔢 Mã captcha: {captcha}";
                    }
                    else
                    {
                        _lblAPICapcha.ForeColor = System.Drawing.Color.Red;
                        _lblAPICapcha.Text =
                            $"{ToVietnameseError(status, message)}\n\n" +
                            $"Status: {status}\n{responseText}";
                    }
                }
                catch (Exception jsonEx)
                {
                    _lblAPICapcha.ForeColor = System.Drawing.Color.OrangeRed;
                    _lblAPICapcha.Text =
                        $"Dữ liệu trả về không hợp lệ\n\n" +
                        $"{jsonEx.Message}\n{responseText}";
                }
            }
            catch (TaskCanceledException)
            {
                _lblAPICapcha.ForeColor = System.Drawing.Color.Red;
                _lblAPICapcha.Text = "Kết nối tới máy chủ bị quá thời gian";
            }
            catch (HttpRequestException ex)
            {
                _lblAPICapcha.ForeColor = System.Drawing.Color.Red;
                _lblAPICapcha.Text =
                    $"Không thể kết nối tới máy chủ API\n\n" +
                    $"{ex.Message}";
            }
            catch (Exception ex)
            {
                _lblAPICapcha.ForeColor = System.Drawing.Color.Red;
                _lblAPICapcha.Text =
                    $"Có lỗi không xác định xảy ra\n\n" +
                    $"{ex}";
            }
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi ComboBox Server API
        /// </summary>
        public void HandleServerApiSelectionChanged(Action<string> onServerApiChanged)
        {
            if (_isLoading) return;

            switch (_cbbServerAPI.SelectedIndex)
            {
                case 0:
                    _txtServerAPI.Text = "http://api.phamgiang.net/captcha/nro?token=";
                    _txtServerAPI.Enabled = false;
                    break;

                case 1:
                    _txtServerAPI.Text = "http://api.tooltvt.com/api?token=";
                    _txtServerAPI.Enabled = false;
                    break;

                default:
                    _txtServerAPI.Enabled = true;
                    return;
            }

            onServerApiChanged?.Invoke(_txtServerAPI.Text);
        }

        /// <summary>
        /// Load API Config từ file
        /// </summary>
        public void LoadApiConfig(string serverApiPath)
        {
            _isLoading = true;

            try
            {
                string serverAPI = File.ReadAllText(serverApiPath);
                if (serverAPI.Contains("phamgiang"))
                    _cbbServerAPI.SelectedIndex = 0;
                else if (serverAPI.Contains("tooltvt"))
                    _cbbServerAPI.SelectedIndex = 1;
                else
                {
                    _cbbServerAPI.SelectedIndex = 2;
                }
                _txtServerAPI.Text = serverAPI;
            }
            catch { }

            _isLoading = false;
        }

        /// <summary>
        /// Chuyển đổi HTTP status code và API status thành message tiếng Việt
        /// </summary>
        private string ToVietnameseError(int status, string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
                return message;

            return status switch
            {
                400 => "Dữ liệu gửi lên không hợp lệ",
                401 => "API Key không hợp lệ hoặc đã hết hạn",
                403 => "Bạn không có quyền sử dụng dịch vụ này",
                404 => "Không tìm thấy dịch vụ captcha",
                429 => "Bạn gửi quá nhiều yêu cầu, vui lòng thử lại sau",
                500 => "Máy chủ xử lý captcha đang gặp lỗi",
                502 => "Không thể kết nối tới máy chủ captcha",
                503 => "Dịch vụ captcha tạm thời không khả dụng",
                _ => "Có lỗi xảy ra khi xử lý captcha"
            };
        }
    }
}