using menu_api.Context;
using menu_api.Models;
using Microsoft.EntityFrameworkCore;

namespace menu_api.Repositories
{
    public class IngredientRepository : IDisposable, IIngredientRepository
    {
        private MenuContext context;
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
            await context.Ingredients.AddAsync(ingredient);
            await context.SaveChangesAsync();
        }

        public async Task DeleteIngredient(Guid ingriedientId)
        {
            Ingredient? ingredient = await context.Ingredients.FindAsync(ingriedientId);
            if (ingredient != null)
            {
                context.Ingredients.Remove(ingredient);
                await context.SaveChangesAsync();
            }
        }
        public async Task UpdateIngredient(Ingredient ingredient)
        {
            context.Ingredients.Update(ingredient);
            await context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
