using menu_api.Context;
using menu_api.Models;
using menu_api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace menu_api.Repositories;

public class CategoryRepository : ICategoryRepository
{

    private readonly MenuContext _context;
    
    public CategoryRepository(MenuContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        return await _context.Categories.ToListAsync();
    }
    
}