using menu_api.Context;
using menu_api.Exceptions;
using menu_api.Models;
using menu_api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace menu_api.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly MenuContext _context;

        public MenuItemRepository(MenuContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenuItem>> GetMenuItems()
        {
            return await _context.MenuItems
                .Include(c => c.Ingredients)
                .Include(item => item.Category)
                .ToListAsync();
        }

        public async Task<MenuItem?> GetMenuItemById(Guid menuItemId)
        {
            return await _context.MenuItems
                .Include(c => c.Ingredients)
                .Include(item => item.Category)
                .FirstOrDefaultAsync(m => m.Id == menuItemId);
        }
        
        public async Task<IEnumerable<MenuItem>> GetMenuItemsByCategoryId(Guid categoryId)
        {
            return await _context.MenuItems
                .Include(item => item.Ingredients)
                .Include(item => item.Category)
                .Where(item => item.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task CreateMenuItem(MenuItem menuItem)
        {
            if (await GetMenuItemById(menuItem.Id) != null)
            { throw new ItemAlreadyExistsException(); }

            await _context.MenuItems.AddAsync(menuItem);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteMenuItem(Guid menuItemId)
        {
            var menuItem = await _context.MenuItems.FindAsync(menuItemId);

            if (menuItem == null)
            { throw new ItemDoesNotExistException();}

            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMenuItem(MenuItem menuItem)
        {
            if (await GetMenuItemById(menuItem.Id) == null)
            { throw new ItemDoesNotExistException(); }

            _context.ChangeTracker.Clear();
            _context.MenuItems.Update(menuItem);
            await _context.SaveChangesAsync();
        }

        
    }
}
