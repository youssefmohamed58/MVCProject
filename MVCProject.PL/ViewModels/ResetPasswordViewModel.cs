using System.ComponentModel.DataAnnotations;

namespace MVCProject.PL.ViewModels
{
	public class ResetPasswordViewModel
	{

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is required")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password Does't Match")]
		public string ConfirmPassword { get; set; }
	}
}
