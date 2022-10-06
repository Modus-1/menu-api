using menu_api.Models;

namespace menu_api.Repositories
{
    public interface IMenuItem_IngredientRepository : IDisposable
    {
        Task AddIngredient(MenuItem_Ingredient menuItem_Ingredient);
        void Dispose();
        Task RemoveIngredient(Guid menuItemId, Guid ingredientId);
        Task RemoveIngredients(Guid menuItemId);
    }
}