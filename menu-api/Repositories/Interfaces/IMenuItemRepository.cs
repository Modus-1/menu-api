using menu_api.Models;

namespace menu_api.Repositories
{
    public interface IMenuItemRepository : IDisposable
    {
        IEnumerable<MenuItem> GetMenuItems();
        MenuItem GetMenuItemByID(Guid menuItemId);
        void InsertMenuItem(MenuItem menuItem);
        void DeleteMenuItem(Guid menuItemId);
        void UpdateMenuItem(MenuItem menuItem);
        void Save();
    }
}
