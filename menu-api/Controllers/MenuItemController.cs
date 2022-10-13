using menu_api.Context;
using menu_api.Models;
using menu_api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace menu_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemRepository menuItemRepository;
        private readonly IMenuItem_IngredientRepository menuItemIngredientRepository;

        public MenuItemController(IMenuItemRepository MenuItemRepo, IMenuItem_IngredientRepository MenuItem_IngredientRepo )
        {
            menuItemRepository = MenuItemRepo;
            menuItemIngredientRepository = MenuItem_IngredientRepo;
        }

        [HttpGet]
        public async Task<IEnumerable<MenuItem>> GetMenuItems()
        {
            return await menuItemRepository.GetMenuItems();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItem>> GetMenuItemByID(Guid id)
        {
            var menuItem = await menuItemRepository.GetMenuItemByID(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            return menuItem;
        }

        [HttpPost]
        public async Task InsertMenuItem(MenuItem menuItem)
        {
            await menuItemRepository.InsertMenuItem(menuItem);
        }

        [HttpDelete("{id}")]
        public async Task DeleteMenuItem(Guid id)
        {
            await menuItemIngredientRepository.RemoveAllIngredients(id);
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
