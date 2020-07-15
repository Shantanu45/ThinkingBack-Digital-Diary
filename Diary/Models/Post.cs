using Microsoft.AspNetCore.Identity;
using System;

namespace Diary.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public DateTime Date { get; set; } = DateTime.Now;
        public IdentityUser User { get; set; }
        public string themeColor { get; set; } = "0, 0%, 100%";
    }
}
