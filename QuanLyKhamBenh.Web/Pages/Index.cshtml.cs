using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace QuanLyKhamBenh.Web.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public string WelcomeMessage { get; set; }

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{
			if (User.Identity?.IsAuthenticated == true)
			{
				// Lấy tên người dùng từ claim. ClaimTypes.Name được thiết lập trong LoginModel
				var userName = User.FindFirstValue(ClaimTypes.Name) ?? User.Identity.Name;
				WelcomeMessage = $"Chào mừng trở lại, {userName}!";
			}
			else
			{
				WelcomeMessage = "Welcome";
			}
		}
	}
}