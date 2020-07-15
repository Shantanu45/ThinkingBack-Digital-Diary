using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Vereyon.Web;

namespace Diary.Controllers
{
    public class AuthController : Controller
    {
        private IFlashMessage _flashMessage;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IFlashMessage flashMessage)
        {
            _flashMessage = flashMessage;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                // sign in
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (signInResult.Succeeded)
                {
                    //_flashMessage.Confirmation("Now You Are Logged In");
                    var role = await _userManager.GetRolesAsync(user);
                    if (role.Any(r => r == "Admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Posts");
                    }
                }
            }
            _flashMessage.Danger("Wrong Email or Password!");

            return RedirectToAction("Index");
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password)
        {
            if (await _userManager.FindByEmailAsync(email) != null)
            {
                _flashMessage.Danger("An Account with this Email already exists, try Log In insted!");
                return RedirectToAction("Index");
            }
            var user = new IdentityUser
            {
                UserName = username,
                Email = email
            };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (signInResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("Index", "Posts");
                }
            }
            _flashMessage.Danger("Sorry! Something is wrong");
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> LogOut()
        {
            //_flashMessage.Confirmation("Now You Are Logged Out");

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");

        }
    }
}
