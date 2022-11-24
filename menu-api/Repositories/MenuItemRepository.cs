﻿using menu_api.Context;
using menu_api.Models;
using Microsoft.EntityFrameworkCore;
using menu_api.Exeptions;

namespace menu_api.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly MenuContext context;

        public MenuItemRepository(MenuContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<MenuItem>> GetMenuItems()
        {
            return await context.MenuItems
                .Include(c => c.Ingredients)
                .Include(item => item.Category)
                .ToListAsync();
        }

        public async Task<MenuItem?> GetMenuItemById(Guid menuItemId)
        {
            return await context.MenuItems
                .Include(c => c.Ingredients)
                .Include(item => item.Category)
                .FirstOrDefaultAsync(m => m.Id == menuItemId);
        }
        
        public async Task<IEnumerable<MenuItem>> GetMenuItemsByCategoryId(Guid categoryId)
        {
            return await context.MenuItems
                .Include(item => item.Ingredients)
                .Include(item => item.Category)
                .Where(item => item.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task CreateMenuItem(MenuItem menuItem)
        {
            if (await GetMenuItemById(menuItem.Id) != null)
            { throw new ItemAlreadyExsistsException(); }

            await context.MenuItems.AddAsync(menuItem);
            await context.SaveChangesAsync();

        }

        public async Task DeleteMenuItem(Guid menuItemId)
        {
            var menuItem = await context.MenuItems.FindAsync(menuItemId);

            if (menuItem == null)
            { throw new ItemDoesNotExistException();}

            context.MenuItems.Remove(menuItem);
            await context.SaveChangesAsync();
        }

        public async Task UpdateMenuItem(MenuItem menuItem)
        {
            if (await GetMenuItemById(menuItem.Id) == null)
            { throw new ItemDoesNotExistException(); }

            context.ChangeTracker.Clear();
            context.MenuItems.Update(menuItem);
            await context.SaveChangesAsync();
        }

        
    }
}
