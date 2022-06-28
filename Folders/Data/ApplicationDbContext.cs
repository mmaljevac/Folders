using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Folders.Models;

namespace Folders.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Folder> Folders { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Folder>().ToTable("Folder");
            modelBuilder.Entity<Permission>().ToTable("Permission");
            modelBuilder.Entity<File>().ToTable("File");

            base.OnModelCreating(modelBuilder);
        }
    }
}
