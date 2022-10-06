using menu_api.Models;

namespace menu_api.Repositories
{
    public interface IMenuItem_IngredientRepository : IDisposable
    {
        void AddIngredient(MenuItem_Ingredient menuItem_Ingredient);
        void Dispose();
        void RemoveIngredient(Guid menuItemId, Guid ingredientId);
        void RemoveIngredients(Guid menuItemId);
    }
}