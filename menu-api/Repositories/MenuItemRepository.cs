using menu_api.Context;
using menu_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace menu_api.Repositories
{
    public class MenuItemRepository : IMenuItemRepository, IDisposable
    {
        private MenuContext context;

        public MenuItemRepository(MenuContext context)
        {
            this.context = context;
        }

        public IEnumerable<MenuItem> GetMenuItems()
        {
            return context.MenuItems.ToList();
        }
        public MenuItem GetMenuItemByID(Guid menuItemId)
        {
            return context.MenuItems.Find(menuItemId);
        }
        public void InsertMenuItem(MenuItem menuItem)
        {
            context.MenuItems.Add(menuItem);
        }
        public void DeleteMenuItem(Guid menuItemId)
        {
            MenuItem menuItem = context.MenuItems.Find(menuItemId);
            context.MenuItems.Remove(menuItem);
        }
        public void UpdateMenuItem(MenuItem menuItem)
        {
            context.Entry(menuItem).State = EntityState.Modified;
        }
        public void Save()
        {
            context.SaveChanges();
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
