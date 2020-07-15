using Diary.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Threading.Tasks;
using Vereyon.Web;

namespace Diary.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private IFlashMessage _flashMessage;
        private AppDbContext _context;
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private IEmailService _emailService;

        public ForgotPasswordController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailService emailService,
            AppDbContext context,
            IFlashMessage flashMessage)
        {
            _flashMessage = flashMessage;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = Url.Action(nameof(AskNewPassword), "ForgotPassword", new { userId = user.Id, code }, Request.Scheme, Request.Host.ToString());
            await _emailService.SendAsync(email, "email verify", $"<a href=\"{link}\">Change Password</a>", true);
            _flashMessage.Warning("Check your Email");
            return View();
        }

        public async Task<IActionResult> AskNewPassword(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (await _userManager.VerifyUserTokenAsync(user, this._userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", code))
            {
                return View(user);
            };
            return RedirectToAction("Index", "Home");       //token invalid
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string newPassword, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            string hashedNewPassword = _userManager.PasswordHasher.HashPassword(user, newPassword);
            UserStore<IdentityUser> store = new UserStore<IdentityUser>(_context);
            await store.SetPasswordHashAsync(user, hashedNewPassword);
            await store.UpdateAsync(user);
            _flashMessage.Confirmation("You Password is changed successfully");
            return RedirectToAction("Index", "Auth");
        }
    }
}
