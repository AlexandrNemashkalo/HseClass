using System;
using HseClass.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HseClass.Core.EF
{
    public sealed class HseClassContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<ClassRoom> ClassRooms { get; set; }

        public DbSet<UserClass> UserClasses { get; set; }
        
        public DbSet<UserLab> UserLabs { get; set; }

        public DbSet<Lab> Labs { get; set; }
        
        public HseClassContext(DbContextOptions<HseClassContext> opt)
            : base(opt)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserClass>()
                .HasKey(uc => new {uc.UserId, TeamId = uc.ClassRoomId});
            
            modelBuilder.Entity<UserLab>()
                .HasKey(uc => new {uc.UserId, uc.LabId});

            modelBuilder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int>[]
                {
                    new()
                    {
                        Id = 1,
                        Name = "student",
                        NormalizedName = "STUDENT"
                    }
                });
            
            modelBuilder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int>[]
                {
                    new()
                    {
                        Id = 2,
                        Name = "teacher",
                        NormalizedName = "TEACHER"
                    }
                });
            
            modelBuilder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int>[]
                {
                    new()
                    {
                        Id = 100,
                        Name = "superAdmin",
                        NormalizedName = "SUPERADMIN"
                    }
                });
            

            base.OnModelCreating(modelBuilder);
        }
    }
}