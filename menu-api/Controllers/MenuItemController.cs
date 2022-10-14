using menu_api.Context;
using menu_api.Models;
using menu_api.Exeptions;
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
                return NotFound("MenuItem Not found");
            }
            return menuItem;
        }

        [HttpPost]
        public async Task<ActionResult> InsertMenuItem(MenuItem menuItem)
        {
            try
            {
                await menuItemRepository.InsertMenuItem(menuItem);
                return Ok();
            }
            catch (ItemAlreadyExsistsExeption)
            {
                return Conflict("MenuItem already exists");
            }
        }

        [HttpDelete("{id}")] 
        public async Task<ActionResult> DeleteMenuItem(Guid id)
        {
            try
            {
                await menuItemIngredientRepository.RemoveAllIngredients(id);
                await menuItemRepository.DeleteMenuItem(id);
                return Ok();
            }
            catch (ItemDoesNotExistExeption)
            {
                return NotFound("MenuItem Not found");
            }
            
        }

        [HttpPost("ingredient")]
        public async Task<ActionResult> AddIngredientToMenuItem(MenuItem_Ingredient menuItem_Ingredient)
        {
            try
            {
                await menuItemIngredientRepository.AddIngredient(menuItem_Ingredient);
                return Ok();
            }
            catch (ItemDoesNotExistExeption ex)
            {
                return NotFound(ex.Message + " Not found");
            }
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateMenuItem(MenuItem menuItem)
        {
            try
            {
                await menuItemRepository.UpdateMenuItem(menuItem);
                return Ok();
            }
            catch (ItemDoesNotExistExeption)
            {
                return NotFound("MenuItem Not found");
            }
        }

        [HttpDelete("ingredient")]
        public async Task<ActionResult> DeleteIngredientFromMenuItem(Guid id, Guid ingredientId)
        {
            try
            {
                await menuItemIngredientRepository.RemoveIngredient(id, ingredientId);
                return Ok();
            }
            catch (ItemDoesNotExistExeption)
            {
                return NotFound("MenuItem_Ingredient Not found");
            }
        }
    }
}
