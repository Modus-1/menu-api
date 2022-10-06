using Microsoft.AspNetCore.Mvc;
using menu_api.Models;
using menu_api.Repositories;
using menu_api.Context;

namespace menu_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemRepository menuItemRepository;
        private readonly IMenuItem_IngredientRepository menuItemIngredientRepository;


        public MenuItemController(MenuContext menuContext)
        {
            menuItemRepository = new MenuItemRepository(menuContext);
            menuItemIngredientRepository = new MenuItem_IngredientRepository(menuContext);
        }

        [HttpGet]
        public async Task<IEnumerable<MenuItem>> GetMenuItems()
        {
            var menuItems = await menuItemRepository.GetMenuItems();
            if (menuItems == null)
            {
                return Enumerable.Empty<MenuItem>();
            }
            else return menuItems; 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItem>> GetMenuItemByID(Guid id)
        {
            var menuItem = await menuItemRepository.GetMenuItemByID(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            else return menuItem;
        }

        [HttpPost]
        public async Task InsertMenuItem(MenuItem menuItem)
        {
            await menuItemRepository.InsertMenuItem(menuItem);
        }


        [HttpDelete("{id}")]
        public async Task DeleteMenuItem(Guid id)
        {
            await menuItemIngredientRepository.RemoveIngredients(id);
            await menuItemRepository.DeleteMenuItem(id);
        }

        [HttpPatch]
        public async Task UpdateMenuItem(MenuItem menuItem)
        {
            await menuItemRepository.UpdateMenuItem(menuItem);
        }



        [HttpPost("ingredient")]
        public async Task AddIngredientToMenuItem(MenuItem_Ingredient menuItem_Ingredient)
        {
            await menuItemIngredientRepository.AddIngredient(menuItem_Ingredient);
        }
        [HttpDelete("ingredient")]
        public async Task DeleteIngredientFromMenuItem(Guid id, Guid ingredientId)
        {
            await menuItemIngredientRepository.RemoveIngredient(id, ingredientId);
        }
    }
}
