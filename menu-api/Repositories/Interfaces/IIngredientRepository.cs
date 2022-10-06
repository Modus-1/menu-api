using menu_api.Models;

namespace menu_api.Repositories
{
    public interface IIngredientRepository : IDisposable
    {
        void DeleteIngredient(Guid ingriedientId);
        IEnumerable<Ingredient> GetAllIngredients();
        Ingredient GetIngredientByID(Guid ingredientId);
        void InsertIngredient(Ingredient ingredient);
        void UpdateIngredient(Ingredient ingredient);
    }
}