using menu_api.Models;

namespace menu_api.Repositories
{
    public interface IMenuItem_IngredientRepository
    {
        Task AddIngredient(MenuItem_Ingredient menuItem_Ingredient);
        Task RemoveIngredient(Guid menuItemId, Guid ingredientId);
        Task RemoveAllIngredients(Guid menuItemId);
    }
}