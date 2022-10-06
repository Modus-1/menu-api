using menu_api.Context;
using menu_api.Models;

namespace menu_api.Repositories
{
    public class MenuItem_IngredientRepository : IMenuItem_IngredientRepository
    {
        private readonly MenuContext context;
        public MenuItem_IngredientRepository(MenuContext context)
        {
            this.context = context;
        }

        //Add ingredient to menuItem
        public async Task AddIngredient(MenuItem_Ingredient menuItem_Ingredient)
        {
            await context.MenuItem_Ingredients.AddAsync(menuItem_Ingredient);
            await context.SaveChangesAsync();
        }

        //remove ingredient from menuItem
        public async Task RemoveIngredient(Guid menuItemId, Guid ingredientId)
        {
            MenuItem_Ingredient? menuItem_Ingredient = await context.MenuItem_Ingredients
                .FindAsync(menuItemId, ingredientId);
            if (menuItem_Ingredient != null)
            {
                context.MenuItem_Ingredients.Remove(menuItem_Ingredient);
                await context.SaveChangesAsync();
            }
        }

        //remove all ingredients from menuItem
        public async Task RemoveIngredients(Guid menuItemId)
        {
            IEnumerable<MenuItem_Ingredient> menuItem_Ingredients = context.MenuItem_Ingredients
                .Where(x => x.MenuItemId == menuItemId);
            context.MenuItem_Ingredients.RemoveRange(menuItem_Ingredients);
            await context.SaveChangesAsync();
        }

    }
}
