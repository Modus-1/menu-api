using menu_api.Models;

namespace menu_api.Repositories
{
    public interface IMenuItemRepository
    {
        Task DeleteMenuItem(Guid menuItemId);
        Task<MenuItem?> GetMenuItemByID(Guid menuItemId);
        Task<IEnumerable<MenuItem>?> GetMenuItems();
        Task InsertMenuItem(MenuItem menuItem);
        Task UpdateMenuItem(MenuItem menuItem);
        Task<IEnumerable<MenuItem>> GetMenuItemsByCategoryId(Guid categoryId);
    }
}