using CookbookAPI.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace CookbookAPI.Data.Context
{
    public class CookbookApiDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;

        public CookbookApiDbContext() { }

        public CookbookApiDbContext(DbContextOptions<CookbookApiDbContext> options, IConfiguration configuration)
        : base(options)
        {
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<User>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {             
                var connectionString = _configuration?.GetConnectionString("CookbookDatabase")
                                       ?? "Server=localhost;Database=CookbookDb;Trusted_Connection=True;TrustServerCertificate=True;";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Main Course" },
                new Category { Id = 2, Name = "Dessert" },
                new Category { Id = 3, Name = "Appetizer" }
            );

            // Seed Users with hashed passwords
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 1, 
                    Name = "Regular User", 
                    Email = "user@mail.com", 
                    Password = _passwordHasher.HashPassword(null, "123456"),
                    Role = Role.RegisteredUser 
                },
                new User 
                { 
                    Id = 2, 
                    Name = "Admin User", 
                    Email = "admin@mail.com", 
                    Password = _passwordHasher.HashPassword(null, "123456"), 
                    Role = Role.Admin 
                }
            );

            // Seed Recipes
            modelBuilder.Entity<Recipe>().HasData(
                new Recipe
                {
                    Id = 1,
                    Name = "Spaghetti Bolognese",
                    Instructions = "1. Cook spaghetti. 2. Make bolognese sauce. 3. Combine and serve.",
                    Ingredients = "Spaghetti, Ground beef, Tomato sauce, Onions, Garlic",
                    CategoryId = 1,
                    Difficulty = "Medium",
                    Duration = 30,
                    IsApproved = true,
                    SubmissionDate = DateTime.UtcNow
                },
                new Recipe
                {
                    Id = 2,
                    Name = "Chocolate Chip Cookies",
                    Instructions = "1. Mix ingredients. 2. Form cookies. 3. Bake for 10-12 minutes.",
                    Ingredients = "Flour, Sugar, Butter, Chocolate chips, Eggs",
                    CategoryId = 2,
                    Difficulty = "Easy",
                    Duration = 25,
                    IsApproved = true,
                    SubmissionDate = DateTime.UtcNow
                },
                new Recipe
                {
                    Id = 3,
                    Name = "Caprese Salad",
                    Instructions = "1. Slice tomatoes and mozzarella. 2. Arrange on plate with basil. 3. Drizzle with olive oil and balsamic.",
                    Ingredients = "Tomatoes, Fresh mozzarella, Basil, Olive oil, Balsamic vinegar",
                    CategoryId = 3,
                    Difficulty = "Easy",
                    Duration = 10,
                    IsApproved = true,
                    SubmissionDate = DateTime.UtcNow
                }
            );

            // Seed Comments
            modelBuilder.Entity<Comment>().HasData(
                new Comment { Id = 1, Content = "Great recipe!", CreatedAt = DateTime.UtcNow, RecipeId = 1, UserId = 1 },
                new Comment { Id = 2, Content = "My family loved it!", CreatedAt = DateTime.UtcNow, RecipeId = 1, UserId = 2 },
                new Comment { Id = 3, Content = "Best cookies ever!", CreatedAt = DateTime.UtcNow, RecipeId = 2, UserId = 1 },
                new Comment { Id = 4, Content = "Simple and delicious!", CreatedAt = DateTime.UtcNow, RecipeId = 3, UserId = 2 }
            );

            // Seed Ratings
            modelBuilder.Entity<Rating>().HasData(
                new Rating { Id = 1, Stars = 5, RecipeId = 1, UserId = 1 },
                new Rating { Id = 2, Stars = 4, RecipeId = 1, UserId = 2 },
                new Rating { Id = 3, Stars = 5, RecipeId = 2, UserId = 1 },
                new Rating { Id = 4, Stars = 5, RecipeId = 3, UserId = 2 }
            );

            // Seed Favorites
            modelBuilder.Entity<Favorite>().HasData(
                new Favorite { Id = 1, RecipeId = 1, UserId = 1 },
                new Favorite { Id = 2, RecipeId = 2, UserId = 1 },
                new Favorite { Id = 3, RecipeId = 3, UserId = 2 }
            );
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
    }
}
