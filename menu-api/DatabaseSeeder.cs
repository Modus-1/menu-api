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

    public void SeedMenuItems()
    {
        List<Category?> categories = GetCategories();
        if(_context.MenuItems.Any()) return;

        if (categories.Any(c => c == null)) return;

        var menuitems = new List<MenuItem>
        {
            new MenuItem
            {
                Name = "Tomaten soep",
                IconUrl = "https://res.cloudinary.com/tkwy-prod-eu/image/upload/ar_1:1,c_thumb,h_340,w_340/f_auto/q_auto/dpr_1.0/v1667725722/static-takeaway-com/images/restaurants/nl/OR710P7N/products/pro_2376_soeptomaatbasilicummedium_500x500px",
                BannerUrl = "https://res.cloudinary.com/tkwy-prod-eu/image/upload/ar_1:1,c_thumb,h_340,w_340/f_auto/q_auto/dpr_1.0/v1667725722/static-takeaway-com/images/restaurants/nl/OR710P7N/products/pro_2376_soeptomaatbasilicummedium_500x500px",
                LongDescription = "Een heerlijke kom 'verse' tomaten soep",
                ShortDescription = "Een heerlijke kom tomaten soep",
                Price = 4.75,
                CategoryId = categories.FirstOrDefault(m => m.Name == "Appetizers")!.Id,
            },

            new MenuItem
            {
                Name = "Pizza Salami",
                IconUrl = "https://res.cloudinary.com/tkwy-prod-eu/image/upload/ar_1:1,c_thumb,h_340,w_340/f_auto/q_auto/dpr_1.0/v1667233573/static-takeaway-com/images/restaurants/nl/31077/products/25671_magicdnerpizza_food_pizzasalami",
                BannerUrl = "https://res.cloudinary.com/tkwy-prod-eu/image/upload/ar_1:1,c_thumb,h_340,w_340/f_auto/q_auto/dpr_1.0/v1667233573/static-takeaway-com/images/restaurants/nl/31077/products/25671_magicdnerpizza_food_pizzasalami",
                LongDescription = "Een pizza met salami.",
                ShortDescription = "Een pizza met salami.",
                Price = 8.99,
                CategoryId = categories.FirstOrDefault(m => m.Name == "Main Courses")!.Id,
            },

            new MenuItem
            {
                Name = "Coca Cola",
                IconUrl = "https://res.cloudinary.com/tkwy-prod-eu/image/upload/ar_1:1,c_thumb,h_340,w_340/f_auto/q_auto/dpr_1.0/v1667233573/static-takeaway-com/images/chains/nl/cocacola/products/coca-cola-33-cl",
                BannerUrl = "https://res.cloudinary.com/tkwy-prod-eu/image/upload/ar_1:1,c_thumb,h_340,w_340/f_auto/q_auto/dpr_1.0/v1667233573/static-takeaway-com/images/chains/nl/cocacola/products/coca-cola-33-cl",
                LongDescription = "Een blikje Coca cola.",
                ShortDescription = "Een blikje Coca cola.",
                Price = 2.00,
                CategoryId = categories.FirstOrDefault(m => m.Name == "Drinks")!.Id,
            },

            new MenuItem
            {
                Name = "Ben and Jerry's Cookie Dough",
                IconUrl = "https://res.cloudinary.com/tkwy-prod-eu/image/upload/ar_1:1,c_thumb,h_340,w_340/f_auto/q_auto/dpr_1.0/v1664958222/static-takeaway-com/images/restaurants/nl/N1R5O7Q/products/633eeb931a8863a7e904d889",
                BannerUrl = "https://res.cloudinary.com/tkwy-prod-eu/image/upload/ar_1:1,c_thumb,h_340,w_340/f_auto/q_auto/dpr_1.0/v1664958222/static-takeaway-com/images/restaurants/nl/N1R5O7Q/products/633eeb931a8863a7e904d889",
                LongDescription = "Een pint Ben en Jerry's Cookie Dough ijs van 465ml.",
                ShortDescription = "Een pint Ben en Jerry's Cookie Dough ijs.",
                Price = 7.25,
                CategoryId = categories.FirstOrDefault(m => m.Name == "Desserts")!.Id,
            }
        };

        _context.MenuItems.AddRange(menuitems);
        _context.SaveChanges();
    }

    private List<Category?> GetCategories()
    {
        List<Category?> categories = new List<Category?>();

        categories.Add(_context.Categories.FirstOrDefault(c => c.Name == "Appetizers"));
        categories.Add(_context.Categories.FirstOrDefault(c => c.Name == "Main Courses"));
        categories.Add(_context.Categories.FirstOrDefault(c => c.Name == "Desserts"));
        categories.Add(_context.Categories.FirstOrDefault(c => c.Name == "Drinks"));

        return categories;

    }
    
}