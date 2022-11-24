using menu_api.Context;
using menu_api.Models;
using Microsoft.EntityFrameworkCore;
using menu_api.Exeptions;
using menu_api.Repositories.Interfaces;

namespace menu_api.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly MenuContext _context;

        public IngredientRepository(MenuContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ingredient>> GetAllIngredients()
        {
            return await _context.Ingredients.ToListAsync();
        }

        public async Task<Ingredient?> GetIngredientById(Guid ingredientId)
        {
            return await _context.Ingredients.FindAsync(ingredientId);
        }

        public async Task CreateIngredient(Ingredient ingredient)
        {
            var foundIngredient = await GetIngredientById(ingredient.Id);

            if (foundIngredient != null)
            { throw new ItemAlreadyExsistsException(); }

            await _context.Ingredients.AddAsync(ingredient);
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteIngredient(Guid ingredientId)
        {
            var ingredient = await _context.Ingredients.FindAsync(ingredientId);

            if (ingredient == null) 
            { throw new ItemDoesNotExistException(); }

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateIngredient(Ingredient ingredient)
        {
            var foundIngredient = await _context.Ingredients.SingleOrDefaultAsync(i => i.Id == ingredient.Id);

            if (foundIngredient == null) 
            { throw new ItemDoesNotExistException(); }

            _context.ChangeTracker.Clear();
            _context.Update(ingredient);
            await _context.SaveChangesAsync();
        }
    }
}
