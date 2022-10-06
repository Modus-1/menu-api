using menu_api.Models;

namespace menu_api.Repositories
{
    public interface IMenuItemRepository : IDisposable
    {
        Task DeleteMenuItem(Guid menuItemId);
        void Dispose();
        Task<MenuItem?> GetMenuItemByID(Guid menuItemId);
        Task<IEnumerable<MenuItem>?> GetMenuItems();
        Task InsertMenuItem(MenuItem menuItem);
        Task UpdateMenuItem(MenuItem menuItem);
    }
}