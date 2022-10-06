using menu_api.Context;
using menu_api.Models;
using Microsoft.EntityFrameworkCore;

namespace menu_api.Repositories
{
    public class IngredientRepository : IIngredientRepository, IDisposable
    {
        private MenuContext context;
        public IngredientRepository(MenuContext context)
        {
            this.context = context;
        }


        public IEnumerable<Ingredient> GetAllIngredients()
        {
            return context.Ingredients.ToList();
        }
        public Ingredient GetIngredientByID(Guid ingredientId)
        {
            return context.Ingredients.Find(ingredientId);
        }
        public void InsertIngredient(Ingredient ingredient)
        {
            context.Ingredients.Add(ingredient);
            context.SaveChanges();
        }

        public void DeleteIngredient(Guid ingriedientId)
        {
            Ingredient ingredient = context.Ingredients.Find(ingriedientId);
            context.Ingredients.Remove(ingredient);
            context.SaveChanges();
        }
        public void UpdateIngredient(Ingredient ingredient)
        {
            context.Entry(ingredient).State = EntityState.Modified;
            context.SaveChanges();
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
