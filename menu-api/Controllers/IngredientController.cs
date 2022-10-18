using Microsoft.AspNetCore.Mvc;
using menu_api.Models;
using menu_api.Repositories;
using menu_api.Context;
using menu_api.Exeptions;

namespace menu_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientRepository _ingredientRepository;

        public IngredientController(IIngredientRepository IngredientRepo)
        {
            _ingredientRepository = IngredientRepo;
        }

        [HttpGet]
        public async Task<IEnumerable<Ingredient>> GetAllIngredients()
        {
            return await _ingredientRepository.GetAllIngredients();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ingredient>> GetIngredientByID(Guid id)
        {
            var ingredient = await _ingredientRepository.GetIngredientByID(id);
            if (ingredient == null)
            {
                return NotFound("Ingredient not found");
            }
            return ingredient;
        }

        [HttpPost]
        public async Task<ActionResult> InsertIngredient(Ingredient ingredient)
        {
            try
            {
                await _ingredientRepository.InsertIngredient(ingredient);
                return Ok();
            }
            catch (ItemAlreadyExsistsException)
            {
                return Conflict("Ingredient already exists");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteIngredient(Guid id)
        {
            try
            {
                await _ingredientRepository.DeleteIngredient(id);
                return Ok();
            }
            catch (ItemDoesNotExistException)
            {
                return NotFound("Ingredient not found");
            }
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateIngredient(Ingredient ingredient)
        {
            try
            {
                await _ingredientRepository.UpdateIngredient(ingredient);
                return Ok();
            }
            catch (ItemDoesNotExistException)
            {
                return NotFound("Ingredient not found");
            }
        }
    }
}
