using menu_api.Context;
using menu_api.Models;

namespace menu_api.Repositories
{
    public class MenuItem_IngredientRepository : IMenuItem_IngredientRepository, IDisposable
    {
        private MenuContext context;
        public MenuItem_IngredientRepository(MenuContext context)
        {
            this.context = context;
        }

        //Add ingredient to menuItem
        public void AddIngredient(MenuItem_Ingredient menuItem_Ingredient)
        {
            context.MenuItem_Ingredients.Add(menuItem_Ingredient);
            context.SaveChanges();
        }

        //remove ingredient from menuItem
        public void RemoveIngredient(Guid menuItemId, Guid ingredientId)
        {
            MenuItem_Ingredient menuItem_Ingredient = context.MenuItem_Ingredients
                .Find(menuItemId, ingredientId);
            context.MenuItem_Ingredients.Remove(menuItem_Ingredient);
            context.SaveChanges();
        }

        //remove all ingredients from menuItem
        public void RemoveIngredients(Guid menuItemId)
        {
            IEnumerable<MenuItem_Ingredient> menuItem_Ingredients = context.MenuItem_Ingredients.Where(x => x.MenuItemId == menuItemId);
            context.MenuItem_Ingredients.RemoveRange(menuItem_Ingredients);
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
