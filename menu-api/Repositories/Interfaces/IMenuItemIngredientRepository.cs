using menu_api.Models;

namespace menu_api.Repositories.Interfaces
{
    public interface IMenuItemIngredientRepository
    {
        Task AddIngredient(MenuItemIngredient menuItemIngredient);
        Task RemoveIngredient(Guid menuItemId, Guid ingredientId);
        Task RemoveAllIngredients(Guid menuItemId);
    }
}