using menu_api.Context;
using menu_api.Models;

namespace menu_api;

public class DatabaseSeeder
{

    private readonly MenuContext _context;

    public DatabaseSeeder(MenuContext context)
    {
        _context = context;
    }

    public void SeedCategories()
    {
        if (_context.Categories.Any())
            return;

        var categories = new List<Category>
        {
            new() {Name = "Appetizers"},
            new() {Name = "Main Courses"},
            new() {Name = "Desserts"},
            new() {Name = "Drinks"}
        };
        
        _context.Categories.AddRange(categories);
        _context.SaveChanges();
    }
    
}