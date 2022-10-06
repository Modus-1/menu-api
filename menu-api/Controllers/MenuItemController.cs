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
        private IMenuItemRepository menuItemRepository;
        private IMenuItem_IngredientRepository menuItemIngredientRepository;


        public MenuItemController(MenuContext menuContext)
        {
            menuItemRepository = new MenuItemRepository(menuContext);
            menuItemIngredientRepository = new MenuItem_IngredientRepository(menuContext);
        }

        [HttpGet]
        public IEnumerable<MenuItem> GetMenuItems()
        {
            return menuItemRepository.GetMenuItems();
        }

        [HttpGet("{id}")]
        public MenuItem GetMenuItemByID(Guid id)
        {
            return menuItemRepository.GetMenuItemByID(id);
            //menuItem.Ingredients = 
        }

        [HttpPost]
        public void InsertMenuItem(MenuItem menuItem)
        {
            menuItemRepository.InsertMenuItem(menuItem);
        }


        [HttpDelete("{id}")]
        public void DeleteMenuItem(Guid id)
        {
            menuItemIngredientRepository.RemoveIngredients(id);
            menuItemRepository.DeleteMenuItem(id);
        }

        [HttpPatch]
        public void UpdateMenuItem(MenuItem menuItem)
        {
            menuItemRepository.UpdateMenuItem(menuItem);
        }



        [HttpPost("ingredient")]
        public void AddIngredientToMenuItem(MenuItem_Ingredient menuItem_Ingredient)
        {
            menuItemIngredientRepository.AddIngredient(menuItem_Ingredient);
        }
        [HttpDelete("ingredient")]
        public void DeleteIngredientFromMenuItem(Guid id, Guid ingredientId)
        {
            menuItemIngredientRepository.RemoveIngredient(id, ingredientId);
        }
    }
}
