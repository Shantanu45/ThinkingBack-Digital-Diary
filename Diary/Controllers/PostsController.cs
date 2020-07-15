using Diary.Data;
using Diary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Vereyon.Web;

namespace Diary.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private IFlashMessage _flashMessage;
        private UserManager<IdentityUser> _userManager;
        private AppDbContext _context;

        public PostsController(AppDbContext context, UserManager<IdentityUser> userManager, IFlashMessage flashMessage)
        {
            _flashMessage = flashMessage;
            _userManager = userManager;
            _context = context;

        }

        //[BindProperty]
        //public Post post { get; set; }

        public async Task<IActionResult> Index(int page=1)
        {

            var user = await _userManager.GetUserAsync(User);
            var userPost = _context.Post.Where(p => p.User.Id == user.Id).OrderByDescending(p => p.Date);
            var model = await PagingList.CreateAsync(userPost, 2, page);
            //_flashMessage.Confirmation("Your confirmation message");

            //var users = await _context.Users.FindAsync(user.Id);

            //var roles = await _userManager.GetRolesAsync(user);
            return View(model);
        }

        public async Task<IActionResult> FullPost(int? id)
        {
            var post = _context.Post.Where(p => p.Id == id).First();
            var user = await _userManager.GetUserAsync(User);
            if(post.User == user)
            {
                return View(post);
            }
            return NotFound();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var post = _context.Post.Where(p => p.Id == id).First();
            var user = await _userManager.GetUserAsync(User);
            if (post.User == user)
            {
                return View(post);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Title, Body, Date, themeColor")]Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }
            _context.Update(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Post post)
        {
            //post.Body = WebUtility.HtmlEncode(post.Body);
            _context.Add(post);
            var user = await _userManager.GetUserAsync(User);
            post.User = user;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Remove(int? id)
        {
            var post = await _context.Post.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);
            if (post.User == user)
            {
                _context.Post.Remove(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            _flashMessage.Danger("You can't delete this");
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Search(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
               var user = await _userManager.GetUserAsync(User);
                List<Post> postsSearched = new List<Post>();

                if(searchString.StartsWith("Date:"))
                {
                    foreach (var p in _context.Post)
                    {
                        if (p.User == user && p.Date.ToString("MMMM yyyy").Contains(searchString.Substring(5).Trim(), StringComparison.CurrentCultureIgnoreCase))
                        {
                            postsSearched.Add(p);
                        }
                    }

                }
                else
                {
                    var postsSearchedByTitle = from p in _context.Post
                                               where p.Title.Contains(searchString) && p.User == user
                                               select p;
                    postsSearched = postsSearchedByTitle.ToList();
                }

                //var date = from p in _context.Post
                //           where p.Date.ToString("MMMM dd").Contains(searchString)
                //           select p;

                return View(postsSearched.ToList());

            }

            return NotFound();

        }


    }
}
