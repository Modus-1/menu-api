namespace menu_api.Context
{
    using Microsoft.EntityFrameworkCore;
    using menu_api.Models;

    public class MenuContext : DbContext
    {
        public MenuContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<MenuItemIngredient> MenuItem_Ingredients { get; set; }
        
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItemIngredient>().HasKey(sc => new { sc.MenuItemId, sc.IngredientId });
        }
    }
}
