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

        public DbSet<User> Users { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}