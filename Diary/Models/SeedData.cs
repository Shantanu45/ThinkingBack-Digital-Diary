using Diary.Data;
using Diary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<AppDbContext>>()))
            {
                if (context.Post.Any())
                {
                    return;   // DB has been seeded
                }

                context.Post.AddRange(
                    new Post
                    {
                        Title = "Rio Bravo",
                        Date = DateTime.Parse("1959-4-15"),
                        Body = "Bravo"
                    },

                    new Post
                    {
                        Title = "Rio Bravo",
                        Date = DateTime.Parse("1959-4-15"),
                        Body = "Bravo"
                    },

                    new Post
                    {
                        Title = "Rio Bravo",
                        Date = DateTime.Parse("1959-4-15"),
                        Body = "Bravo"
                    },

                    new Post
                    {
                        Title = "Rio Bravo",
                        Date = DateTime.Parse("1959-4-15"),
                        Body = "Bravo"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
