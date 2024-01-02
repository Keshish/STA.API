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
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        #endregion

        public BaseDataContext(DbContextOptions<BaseDataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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


            modelBuilder.Entity<Parent>()
                .HasMany(p => p.Students)
                .WithOne(s => s.Parent)
                .HasForeignKey(s => s.ParentId)
                .IsRequired();

            modelBuilder.Entity<StudentCourse>()
       .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);
        }
    }
}
