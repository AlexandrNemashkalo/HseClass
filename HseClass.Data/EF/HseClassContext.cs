using System;
using HseClass.Core.Entities;
using HseClass.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HseClass.Data.EF
{
    public sealed class HseClassContext : IdentityDbContext<UserEntity, IdentityRole<int>, int>
    {
        public DbSet<ClassRoomEntity> ClassRooms { get; set; }

        public DbSet<UserClassEntity> UserClasses { get; set; }
        
        public DbSet<SolutionLabEntity> UserLabs { get; set; }

        public DbSet<LabEntity> Labs { get; set; }
        
        public DbSet<TaskLabEntity> TaskLabs { get; set; }
        
        public DbSet<LabStatusEntity> LabStatuses { get; set; }
        
        public DbSet<SolutionLabEntity> SolutionLabs { get; set; }
         
        
        public HseClassContext(DbContextOptions<HseClassContext> opt)
            : base(opt)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserClassEntity>()
                .HasKey(uc => new {UserId = uc.UserId, ClassRoomId = uc.ClassRoomId});
            
            modelBuilder.Entity<SolutionLabEntity>()
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