using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuanLyKhamBenh.Web.Pages
{
    public class LogoutModel : PageModel
    {
        // OnGet is not used for logout to prevent accidental logouts from a simple link click.
        // Logout should be an explicit user action, like a POST request from a button.
        public IActionResult OnGet()
        {
            return RedirectToPage("/Login");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Xóa cookie xác thực
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Chuyển hướng người dùng về trang chủ
            return RedirectToPage("/Login");
        }
    }
}
