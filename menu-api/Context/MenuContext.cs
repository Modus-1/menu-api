namespace menu_api.Context
{
    using Microsoft.EntityFrameworkCore;
    using menu_api.Models;


    public class MenuContext : DbContext
    {
        public MenuContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<MenuItem_Ingredient> MenuItem_Ingredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem_Ingredient>().HasKey(sc => new { sc.MenuItemId, sc.IngredientId });
        }
    }
}
