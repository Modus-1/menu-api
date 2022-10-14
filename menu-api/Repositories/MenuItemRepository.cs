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

        public async Task<IEnumerable<MenuItem>?> GetMenuItems()
        {
            return await context.MenuItems
                .Include(c => c.Ingredients)
                .ToListAsync();
        }

        public async Task<MenuItem?> GetMenuItemByID(Guid menuItemId)
        {
            return await context.MenuItems
                .Include(c => c.Ingredients)
                .FirstOrDefaultAsync(m => m.Id == menuItemId);
        }

        public async Task InsertMenuItem(MenuItem menuItem)
        {
            try
            {
                await context.MenuItems.AddAsync(menuItem);
                await context.SaveChangesAsync();
            }
            catch (System.ArgumentException)
            {
                throw new ItemAlreadyExsistsExeption();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                throw new ItemAlreadyExsistsExeption();
            }
            
        }

        public async Task DeleteMenuItem(Guid menuItemId)
        {
            MenuItem? menuItem = await context.MenuItems.FindAsync(menuItemId);
            if (menuItem != null)
            {
                context.MenuItems.Remove(menuItem);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new ItemDoesNotExistExeption();
            }
        }

        public async Task UpdateMenuItem(MenuItem menuItem)
        {
            try
            {
                context.MenuItems.Update(menuItem);
                await context.SaveChangesAsync();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                throw new ItemDoesNotExistExeption();
            }
        }
    }
}
