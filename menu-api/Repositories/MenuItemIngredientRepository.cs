using menu_api.Context;
using menu_api.Exceptions;
using menu_api.Models;
using menu_api.Repositories.Interfaces;

namespace menu_api.Repositories
{
    public class MenuItemIngredientRepository : IMenuItemIngredientRepository
    {
        private readonly MenuContext _context;

        public MenuItemIngredientRepository(MenuContext context)
        {
            this._context = context;
        }

        //Add ingredient to menuItem
        public async Task AddIngredient(MenuItemIngredient menuItemIngredient)
        {
            var ingredient = await _context.Ingredients.FindAsync(menuItemIngredient.IngredientId);
            var menuItem = await _context.MenuItems.FindAsync(menuItemIngredient.MenuItemId);

            var foundMenuItemIngredient = await _context.MenuItem_Ingredients.FindAsync(menuItemIngredient.MenuItemId, menuItemIngredient.IngredientId);

            if (foundMenuItemIngredient != null)
            { throw new ItemAlreadyExistsException(); }

            if (menuItem == null)
            { throw new ItemDoesNotExistException("MenuItem"); }

            if (ingredient == null)
            { throw new ItemDoesNotExistException("Ingredient"); }

            await _context.MenuItem_Ingredients.AddAsync(menuItemIngredient);
            await _context.SaveChangesAsync();
        }

        //remove ingredient from menuItem
        public async Task RemoveIngredient(Guid menuItemId, Guid ingredientId)
        {
            var menuItemIngredient = await _context.MenuItem_Ingredients
                .FindAsync(menuItemId, ingredientId);

            if (menuItemIngredient == null)
            { throw new ItemDoesNotExistException(); }

            _context.MenuItem_Ingredients.Remove(menuItemIngredient);
            await _context.SaveChangesAsync();
        }

        //remove all ingredients from menuItem
        public async Task RemoveAllIngredients(Guid menuItemId)
        {
            var menuItem = await _context.MenuItems.FindAsync(menuItemId);

            if (menuItem == null)
            { throw new ItemDoesNotExistException("MenuItem"); }

            IEnumerable<MenuItemIngredient> menuItemIngredients = _context.MenuItem_Ingredients
                .Where(x => x.MenuItemId == menuItemId);
            _context.MenuItem_Ingredients.RemoveRange(menuItemIngredients);
            await _context.SaveChangesAsync();
        }
    }
}
