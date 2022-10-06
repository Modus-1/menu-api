using menu_api.Models;

namespace menu_api.Repositories
{
    public interface IIngredientRepository : IDisposable
    {
        Task DeleteIngredient(Guid ingriedientId);
        void Dispose();
        Task<IEnumerable<Ingredient>?> GetAllIngredients();
        Task<Ingredient?> GetIngredientByID(Guid ingredientId);
        Task InsertIngredient(Ingredient ingredient);
        Task UpdateIngredient(Ingredient ingredient);
    }
}