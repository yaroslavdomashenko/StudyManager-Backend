using Microsoft.EntityFrameworkCore;
using StudyManager.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyManager.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Students)
                .WithMany(c => c.Courses);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.User)
                .WithMany(c => c.CreatedCourses)
                .HasForeignKey(c => c.UserId);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<HomeworkReply> HomeworkReplies { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
