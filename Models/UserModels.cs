using System.ComponentModel.DataAnnotations;

namespace MOM_Project.Models
{
	public class UserLoginModel
	{
		[Required(ErrorMessage = "Username is required.")]
		public string UserName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password is required.")]
		public string Password { get; set; } = string.Empty;
	}

	public class UserRegisterModel
	{
		[Required(ErrorMessage = "Username is required.")]
		public string UserName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Mobile number is required.")]
		public string MobileNo { get; set; } = string.Empty;

		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Enter a valid email.")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password is required.")]
		public string Password { get; set; } = string.Empty;

		public string? Address { get; set; }
	}
}
