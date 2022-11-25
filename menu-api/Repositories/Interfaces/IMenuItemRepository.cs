using menu_api.Models;

namespace menu_api.Repositories.Interfaces
{
    public interface IMenuItemRepository
    {
        Task DeleteMenuItem(Guid menuItemId);
        Task<MenuItem?> GetMenuItemById(Guid menuItemId);
        Task<IEnumerable<MenuItem>> GetMenuItems();
        Task CreateMenuItem(MenuItem menuItem);
        Task UpdateMenuItem(MenuItem menuItem);
        Task<IEnumerable<MenuItem>> GetMenuItemsByCategoryId(Guid categoryId);
    }
}