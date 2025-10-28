using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKhamBenh.Core.DTOs
{
	public class LoginRequest
	{
		// Thêm ? để cho phép giá trị là null (vì chỉ 1 trong 2 được gửi)
		public string? Phone { get; set; }
		public string? Username { get; set; }

		// Mật khẩu luôn bắt buộc
		public string Password { get; set; }
	}
}