using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCProject.PL.Helper;
using MVCProject.PL.ViewModels;
using Project.DAL.Models;

namespace MVCProject.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}
        
        
        #region Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel RegisterVM)
        {
            if (ModelState.IsValid)
            {
                var Register = new ApplicationUser()
                {
                    UserName = RegisterVM.Email.Split('@')[0],
                    FName = RegisterVM.FName,
                    LName = RegisterVM.LName,
                    Email = RegisterVM.Email,
                    IsAgree = RegisterVM.IsAgree,
                };

                var Result = await _userManager.CreateAsync(Register, RegisterVM.Password);
                if (Result.Succeeded)
                    return RedirectToAction(nameof(Login));
                else
                    foreach (var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);

            }
            return View(RegisterVM);

        }

        #endregion

        #region Login

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModelView LoginVM)
        {
            if (ModelState.IsValid)
            {
                var EmailChecked = await _userManager.FindByEmailAsync(LoginVM.Email);
                if (EmailChecked is not null)
                {
                    var PassChecked = await _userManager.CheckPasswordAsync(EmailChecked, LoginVM.Password);
                    if (PassChecked is true)
                    {
                        var result = await _signInManager.PasswordSignInAsync(EmailChecked, LoginVM.Password, LoginVM.RememberMe, false);
                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Wrong Password");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email is not find");
                }
            }
            return View(LoginVM);
        }

        #endregion

        #region SignOut

        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        } 
        #endregion

        public IActionResult ForgetPassword()
        {
            return View();
        }

        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);

				if (user != null)
				{
                    var Token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = user.Email, token = Token }, Request.Scheme);

					var email = new Email()
					{
						To = model.Email,
						Subject = "Reset Passwword",
						Body = ResetPasswordLink
                    };
                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckInbox));
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Email is not valid");
				}
			}

			return RedirectToAction(nameof(ForgetPassword), model);

		}

        public IActionResult CheckInbox()
        {
            return View();
        }

        public IActionResult ResetPassword(string email, string Token)
        {
            TempData["Email"] = email;
            TempData["Token"] = Token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                string email = TempData["Email"] as string;
                string token = TempData["token"] as string;
                var user = await _userManager.FindByEmailAsync(email);
                var Result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                if(Result.Succeeded)
                   return RedirectToAction(nameof(Login));
                else
                    foreach (var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

    }
}
