using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Threading.Tasks;
using Vereyon.Web;

namespace Diary.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private IFlashMessage _flashMessage;
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private IEmailService _emailService;

        public UserController(
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            IEmailService emailService,
            IFlashMessage flashMessage)
        {
            _flashMessage = flashMessage;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            return View(currentUser);
        }

        //public IActionResult ChangePassword()
        //{
        //    return View();
        //}

        //public async Task<IActionResult> SendResetEmail()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    var email = user.Email;
        //    var code = await _userManager.GeneratePasswordResetTokenAsync(user);

        //    var link = Url.Action(nameof(AskNewPassword), "User", new { userId = user.Id, code }, Request.Scheme, Request.Host.ToString());
        //    await _emailService.SendAsync(email, "email verify", $"<a href=\"{link}\">Change Password</a>", true);
        //    return View();
        //}


        public IActionResult AskNewPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string newPassword, string currentPassword)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                _flashMessage.Confirmation("Your password is changed successfully!");
                return RedirectToAction("Index");
            }
            return RedirectToAction("AskNewPassword");
        }
    }
}
