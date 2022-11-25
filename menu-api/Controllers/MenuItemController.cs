using menu_api.Exceptions;
using menu_api.Models;
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

        public MenuItemController(IMenuItemRepository menuItemRepository, IMenuItemIngredientRepository menuItemIngredientRepository, ICategoryRepository categoryRepository)
        {
            _menuItemRepository = menuItemRepository;
            _menuItemIngredientRepository = menuItemIngredientRepository;
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Gets all menu items from the datastore.
        /// </summary>
        /// <returns>A list of all menu items.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IEnumerable<MenuItem>> GetMenuItems()
        {
            return await _menuItemRepository.GetMenuItems();
        }

        /// <summary>
        /// Gets a menu item by id from the datastore.
        /// </summary>
        /// <param name="id">The id associated with the menu item.</param>
        /// <returns>When found, the menu item associated with the given id.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetMenuItemById(Guid id)
        {
            var menuItem = await _menuItemRepository.GetMenuItemById(id);
            if (menuItem == null)
            {
                return NotFound("MenuItem Not found");
            }
            return Ok(menuItem);
        }

        /// <summary>
        /// Gets all menu items with a specific category by id.
        /// </summary>
        /// <param name="categoryId">The id associated with the specific category.</param>
        /// <returns>A list of all menu items associated with the given category id.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("/category/{categoryId:guid}")]
        public async Task<IEnumerable<MenuItem>> GetMenuItemByCategoryId(Guid categoryId)
        {
            return await _menuItemRepository.GetMenuItemsByCategoryId(categoryId);
        }

        /// <summary>
        /// Adds a new menu item to the datastore.
        /// </summary>
        /// <param name="menuItem">The menu item to add.</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> CreateMenuItem(MenuItem menuItem)
        {
            if ((await _categoryRepository.GetAllCategories()).All(category => category.Id != menuItem.CategoryId))
            {
                return BadRequest("Category does not exist.");
            }
            
            try
            {
                await _menuItemRepository.CreateMenuItem(menuItem);
                return Ok();
            }
            catch (ItemAlreadyExistsException)
            {
                return Conflict("MenuItem already exists");
            }
        }

        /// <summary>
        /// Deletes a menu item from the datastore by id.
        /// </summary>
        /// <param name="id">The id associated with the menu item to delete.</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:guid}")] 
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

        /// <summary>
        /// Updates the information associated with a specific menu item.
        /// </summary>
        /// <param name="menuItem">The new information to replace the menu item with.</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        
        /// <summary>
        /// Adds an ingredient to a specific menu item.
        /// </summary>
        /// <param name="menuItemIngredient">The new ingredient to add to the menu item.</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("ingredient")]
        public async Task<ActionResult> AddIngredientToMenuItem(MenuItemIngredient menuItemIngredient)
        {
            try
            {
                await _menuItemIngredientRepository.AddIngredient(menuItemIngredient);
                return Ok();
            }
            catch (ItemDoesNotExistException ex)
            {
                return NotFound(ex.Message + " Not found");
            }
        }

        /// <summary>
        /// Deletes an ingredient from a specific menu item by id.
        /// </summary>
        /// <param name="menuItemId">The menu item id where the ingredient needs to be removed from.</param>
        /// <param name="ingredientId">The id of the ingredients to be removed from the menu item.</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("ingredient")]
        public async Task<ActionResult> DeleteIngredientFromMenuItem(Guid menuItemId, Guid ingredientId)
        {
            try
            {
                await _menuItemIngredientRepository.RemoveIngredient(menuItemId, ingredientId);
                return Ok();
            }
            catch (ItemDoesNotExistException)
            {
                return NotFound("Ingredient not found in menu item.");
            }
        }
    }
}
