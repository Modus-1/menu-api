using menu_api.Models;

namespace menu_api.Repositories
{
    public interface IIngredientRepository
    {
        Task DeleteIngredient(Guid ingriedientId);
        Task<IEnumerable<Ingredient>?> GetAllIngredients();
        Task<Ingredient?> GetIngredientByID(Guid ingredientId);
        Task InsertIngredient(Ingredient ingredient);
        Task UpdateIngredient(Ingredient ingredient);
    }
}