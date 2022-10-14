using menu_api.Context;
using menu_api.Models;
using Microsoft.EntityFrameworkCore;
using menu_api.Exeptions;

namespace menu_api.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly MenuContext context;

        public IngredientRepository(MenuContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Ingredient>?> GetAllIngredients()
        {
            return await context.Ingredients.ToListAsync();
        }

        public async Task<Ingredient?> GetIngredientByID(Guid ingredientId)
        {
            return await context.Ingredients.FindAsync(ingredientId);
        }

        public async Task InsertIngredient(Ingredient ingredient)
        {
            if (await GetIngredientByID(ingredient.Id) == null)
            {
                await context.Ingredients.AddAsync(ingredient);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new ItemAlreadyExsistsExeption();
            }
        }
        public async Task DeleteIngredient(Guid ingriedientId)
        {
            Ingredient? ingredient = await context.Ingredients.FindAsync(ingriedientId);
            if (ingredient != null)
            {
                context.Ingredients.Remove(ingredient);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new ItemDoesNotExistExeption();
            }
        }

        public async Task UpdateIngredient(Ingredient ingredient)
        {

            

            var _ingredient = await context.Ingredients.SingleOrDefaultAsync(i => i.Id == ingredient.Id);

            if (_ingredient != null)
            {
                context.ChangeTracker.Clear();
                context.Update(ingredient);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new ItemDoesNotExistExeption();
            }
        }
    }
}
