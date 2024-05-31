namespace AgriEnergyConnect.Data
{
    using AgriEnergyConnect.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System;

    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<FarmerProfile> FarmerProfiles { get; set; }
        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; } 
        public DbSet<Product> Products { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FarmerProfile>()
                .HasOne(fp => fp.User)
                .WithMany()
                .HasForeignKey(fp => fp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeProfile>() // Configuring EmployeeProfile entity
                .HasOne(ep => ep.User)
                .WithMany()
                .HasForeignKey(ep => ep.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed data for Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, UserName = "farmer1", NormalizedUserName = "FARMER1", Email = "farmer1@example.com", NormalizedEmail = "FARMER1@EXAMPLE.COM", EmailConfirmed = true, PasswordHash = "AQAAAAEAACcQAAAAEBByYXqN+kvJZNRMoWf7mG+/cAYFN3fZkdpXdsJYh0j7d7zjvKi3kLg2OXKHfx99iA==", SecurityStamp = "FARMER1", ConcurrencyStamp = "1", Role = "Farmer" },
                new User { Id = 2, UserName = "employee1", NormalizedUserName = "EMPLOYEE1", Email = "employee1@example.com", NormalizedEmail = "EMPLOYEE1@EXAMPLE.COM", EmailConfirmed = true, PasswordHash = "AQAAAAEAACcQAAAAEBByYXqN+kvJZNRMoWf7mG+/cAYFN3fZkdpXdsJYh0j7d7zjvKi3kLg2OXKHfx99iA==", SecurityStamp = "EMPLOYEE1", ConcurrencyStamp = "2", Role = "Employee" }
            );

            // Seed data for FarmerProfiles
            modelBuilder.Entity<FarmerProfile>().HasData(
                new FarmerProfile { Id = 1, Name = "Farmer Kat", Location = "Irene Dairy Farm", ContactInfo = "0825754231", UserId = 1, ImagePath = "/images/default_image.jpg", Certifications = "None", FarmSize = 0, TypeOfFarming = "Mixed", YearsOfExperience = 0 }
            );

            // Seed data for EmployeeProfiles
            modelBuilder.Entity<EmployeeProfile>().HasData(
                new EmployeeProfile { Id = 1, Department = "Sales", Position = "Manager", UserId = 2 }
            );

            // Seed data for Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Tomatoes", Category = "Vegetables", ProductionDate = DateTime.Now, FarmerProfileId = 1, ImageUri = "/images/tomatoes.png" }
            );
        }
    }
}
