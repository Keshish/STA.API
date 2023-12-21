using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using STA.API.Models.Authentication;
using STA.API.Models.Users;

namespace STA.API.Models.DbContext
{
    public class BaseDataContext : IdentityDbContext<ApplicationUser>
    {

        #region DbSets
        public DbSet<Supervisor> Supervisors { get; set; }
        public DbSet<Assistant> Assistants { get; set; }
        public DbSet<Parent> Parents { get; set; }
        #endregion


        public BaseDataContext(DbContextOptions<BaseDataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships for User, Supervisor, Assistant, and Parent
            modelBuilder.Entity<Supervisor>()
                .HasOne(s => s.User)
                .WithOne(u => u.Supervisor)
                .HasForeignKey<Supervisor>(s => s.UserId);

            modelBuilder.Entity<Assistant>()
            .HasOne(s => s.User)
            .WithOne(u => u.Assistant)
            .HasForeignKey<Assistant>(s => s.UserId);

            modelBuilder.Entity<Parent>()
            .HasOne(s => s.User)
            .WithOne(u => u.Parent)
            .HasForeignKey<Parent>(s => s.UserId);

        }



    }
}
