using Microsoft.AspNetCore.Mvc;
using menu_api.Models;
using menu_api.Repositories;
using menu_api.Context;

namespace menu_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientRepository ingredientRepository;

        public IngredientController(MenuContext context)
        {
            ingredientRepository = new IngredientRepository(context);
        }

        [HttpGet]
        public async Task<IEnumerable<Ingredient>> GetAllIngredients()
        {
            var ingredients = await ingredientRepository.GetAllIngredients();
            if (ingredients == null)
            {
                return Enumerable.Empty<Ingredient>();
            }
            return ingredients;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ingredient>> GetIngredientById(Guid id)
        {
            var ingredient = await ingredientRepository.GetIngredientByID(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            return ingredient;
        }

        [HttpPost]
        public async Task InsertIngredient(Ingredient ingredient)
        {
            await ingredientRepository.InsertIngredient(ingredient);
        }

        [HttpDelete("{id}")]
        public async Task DeleteIngredient(Guid id)
        {
            await ingredientRepository.DeleteIngredient(id);
        }

        [HttpPatch]
        public async Task UpdateIngredient(Ingredient ingredient)
        {
            await ingredientRepository.UpdateIngredient(ingredient);
        }
    }
}
