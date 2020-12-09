using BlazorApp.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Activity> Activity { get; set; }
    }
}