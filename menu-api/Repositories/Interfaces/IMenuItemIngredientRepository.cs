using menu_api.Models;

namespace menu_api.Repositories
{
    public interface IMenuItemIngredientRepository
    {
        Task AddIngredient(MenuItemIngredient menuItem_Ingredient);
        Task RemoveIngredient(Guid menuItemId, Guid ingredientId);
        Task RemoveAllIngredients(Guid menuItemId);
    }
}