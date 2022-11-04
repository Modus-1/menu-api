using menu_api.Context;
using menu_api.Models;
using menu_api.Exeptions;
using menu_api.Repositories;
using menu_api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace menu_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IMenuItemIngredientRepository _menuItemIngredientRepository;
        private readonly ICategoryRepository _categoryRepository;

        public MenuItemController(IMenuItemRepository MenuItemRepo, IMenuItemIngredientRepository MenuItem_IngredientRepo, ICategoryRepository categoryRepository)
        {
            _menuItemRepository = MenuItemRepo;
            _menuItemIngredientRepository = MenuItem_IngredientRepo;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<MenuItem>> GetMenuItems()
        {
            return await _menuItemRepository.GetMenuItems();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItem>> GetMenuItemByID(Guid id)
        {
            var menuItem = await _menuItemRepository.GetMenuItemByID(id);
            if (menuItem == null)
            {
                return NotFound("MenuItem Not found");
            }
            return menuItem;
        }

        [HttpGet("/category/{categoryId}")]
        public async Task<IEnumerable<MenuItem>> GetMenuItemByCategoryId(Guid categoryId)
        {
            return await _menuItemRepository.GetMenuItemsByCategoryId(categoryId);
        }

        [HttpPost]
        public async Task<ActionResult> InsertMenuItem(MenuItem menuItem)
        {
            if ((await _categoryRepository.GetAllCategories()).All(category => category.Id != menuItem.CategoryId))
            {
                return BadRequest("Category does not exist.");
            }
            
            try
            {
                await _menuItemRepository.InsertMenuItem(menuItem);
                return Ok();
            }
            catch (ItemAlreadyExsistsException)
            {
                return Conflict("MenuItem already exists");
            }
        }

        [HttpDelete("{id}")] 
        public async Task<ActionResult> DeleteMenuItem(Guid id)
        {
            try
            {
                await _menuItemIngredientRepository.RemoveAllIngredients(id);
                await _menuItemRepository.DeleteMenuItem(id);
                return Ok();
            }
            catch (ItemDoesNotExistException)
            {
                return NotFound("MenuItem Not found");
            }
            
        }

        [HttpPost("ingredient")]
        public async Task<ActionResult> AddIngredientToMenuItem(MenuItemIngredient menuItem_Ingredient)
        {
            try
            {
                await _menuItemIngredientRepository.AddIngredient(menuItem_Ingredient);
                return Ok();
            }
            catch (ItemDoesNotExistException ex)
            {
                return NotFound(ex.Message + " Not found");
            }
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateMenuItem(MenuItem menuItem)
        {
            try
            {
                await _menuItemRepository.UpdateMenuItem(menuItem);
                return Ok();
            }
            catch (ItemDoesNotExistException)
            {
                return NotFound("MenuItem Not found");
            }
        }

        [HttpDelete("ingredient")]
        public async Task<ActionResult> DeleteIngredientFromMenuItem(Guid id, Guid ingredientId)
        {
            try
            {
                await _menuItemIngredientRepository.RemoveIngredient(id, ingredientId);
                return Ok();
            }
            catch (ItemDoesNotExistException)
            {
                return NotFound("MenuItem_Ingredient Not found");
            }
        }
    }
}
