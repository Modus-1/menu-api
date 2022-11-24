using menu_api.Models;

namespace menu_api.Repositories.Interfaces
{
    public interface IIngredientRepository
    {
        Task DeleteIngredient(Guid ingredientId);
        Task<IEnumerable<Ingredient>> GetAllIngredients();
        Task<Ingredient?> GetIngredientById(Guid ingredientId);
        Task CreateIngredient(Ingredient ingredient);
        Task UpdateIngredient(Ingredient ingredient);
    }
}