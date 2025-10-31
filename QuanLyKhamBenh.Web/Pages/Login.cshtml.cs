using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace QuanLyKhamBenh.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public LoginInput Input { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = "/")
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = "/")
        {
            ReturnUrl = returnUrl;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Tạo HttpClient để gọi API
            var httpClient = _httpClientFactory.CreateClient("Api");

            try
            {
                // Gửi yêu cầu đăng nhập đến API
                var response = await httpClient.PostAsJsonAsync("/api/auth/login", Input);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    if (string.IsNullOrEmpty(apiResponse?.Token))
                    {
                        ModelState.AddModelError(string.Empty, "API did not return a token.");
                        return Page();
                    }

                    // Giải mã token để lấy claims
                    var claims = ParseClaimsFromJwt(apiResponse.Token);
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                    var principal = new ClaimsPrincipal(identity);

                    // Cấu hình cookie để không lưu lại sau khi tắt trình duyệt
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = false, // <-- Dòng này quyết định cookie sẽ bị xóa khi đóng trình duyệt
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(3) // Giữ thời gian hết hạn đồng bộ với token
                    };

                    // Tạo cookie xác thực với cấu hình đã chỉ định
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, 
                        principal, 
                        authProperties);

                    // === THAY ĐỔI LOGIC CHUYỂN HƯỚNG TẠI ĐÂY ===

                    // Tìm claim chứa vai trò (role)
                    var roleClaim = principal.FindFirst(ClaimTypes.Role);
                    string landingPage = "/"; // Trang mặc định

                    if (roleClaim != null)
                    {
                        switch (roleClaim.Value)
                        {
                            case "Admin":
                                landingPage = "/Admin/Index";
                                break;
                            case "Doctor":
                                landingPage = "/Doctor/Index";
                                break;
                            case "Patient":
                                landingPage = "/Patient/Index";
                                break;
                        }
                    }

                    // Ưu tiên returnUrl nếu có, nếu không thì dùng trang của vai trò
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1)
                    {
                        return LocalRedirect(returnUrl);
                    }
                    
                    return LocalRedirect(landingPage);
                }
                else
                {
                    // Lấy thông báo lỗi từ API nếu có
                    string errorMessage = "Login failed.";
                    if (response.Content.Headers.ContentLength > 0)
                    {
                        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                        errorMessage = errorResponse?.Message ?? errorMessage;
                    }
                    ModelState.AddModelError(string.Empty, errorMessage);
                    return Page();
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Network error: {ex.Message}");
                return Page();
            }
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            return token.Claims;
        }
    }

    // DTO để gửi dữ liệu đến API
    public class LoginInput
    {
        public string? Phone { get; set; }
        public string? Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

    // DTO để nhận token từ API
    public class LoginResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }

    // DTO để nhận thông báo lỗi từ API
    public class ErrorResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
