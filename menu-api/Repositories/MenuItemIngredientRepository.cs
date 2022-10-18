using menu_api.Context;
using menu_api.Exeptions;
using menu_api.Models;

namespace menu_api.Repositories
{
    public class MenuItemIngredientRepository : IMenuItemIngredientRepository
    {
        private readonly MenuContext context;

        public MenuItemIngredientRepository(MenuContext context)
        {
            this.context = context;
        }

        //Add ingredient to menuItem
        public async Task AddIngredient(MenuItemIngredient menuItem_Ingredient)
        {
            var ingredient = await context.Ingredients.FindAsync(menuItem_Ingredient.IngredientId);
            var menuItem = await context.MenuItems.FindAsync(menuItem_Ingredient.MenuItemId);

            var _menuItem_Ingredient = await context.MenuItem_Ingredients.FindAsync(menuItem_Ingredient.MenuItemId, menuItem_Ingredient.IngredientId);

            if (_menuItem_Ingredient != null)
            { throw new ItemAlreadyExsistsException(); }

            else if (menuItem == null)
            { throw new ItemDoesNotExistException("MenuItem"); }

            else if (ingredient == null)
            { throw new ItemDoesNotExistException("Ingredient"); }

            await context.MenuItem_Ingredients.AddAsync(menuItem_Ingredient);
            await context.SaveChangesAsync();
        }

        //remove ingredient from menuItem
        public async Task RemoveIngredient(Guid menuItemId, Guid ingredientId)
        {
            MenuItemIngredient? menuItem_Ingredient = await context.MenuItem_Ingredients
                .FindAsync(menuItemId, ingredientId);

            if (menuItem_Ingredient == null)
            {
                throw new ItemDoesNotExistException();
            }

            context.MenuItem_Ingredients.Remove(menuItem_Ingredient);
            await context.SaveChangesAsync();
        }

        //remove all ingredients from menuItem
        public async Task RemoveAllIngredients(Guid menuItemId)
        {
            var menuItem = await context.MenuItems.FindAsync(menuItemId);

            if (menuItem == null)
            { throw new ItemDoesNotExistException("MenuItem"); }

            IEnumerable<MenuItemIngredient> menuItem_Ingredients = context.MenuItem_Ingredients
                .Where(x => x.MenuItemId == menuItemId);
            context.MenuItem_Ingredients.RemoveRange(menuItem_Ingredients);
            await context.SaveChangesAsync();
        }
    }
}
