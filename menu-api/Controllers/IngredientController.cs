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
        private IIngredientRepository ingredientRepository;

        public IngredientController(MenuContext context)
        {
            ingredientRepository = new IngredientRepository(context);
        }

        [HttpGet]
        public IEnumerable<Ingredient> GetAllIngredients()
        {
            return ingredientRepository.GetAllIngredients();
        }

        [HttpGet("{id}")]
        public Ingredient GetIngredientById(Guid id)
        {
            return ingredientRepository.GetIngredientByID(id);
        }

        [HttpPost]
        public void InsertIngredient(Ingredient ingredient)
        {
            ingredientRepository.InsertIngredient(ingredient);
        }

        [HttpDelete("{id}")]
        public void DeleteIngredient(Guid id)
        {
            ingredientRepository.DeleteIngredient(id);
        }

        [HttpPatch]
        public void UpdateIngredient(Ingredient ingredient)
        {
            ingredientRepository.UpdateIngredient(ingredient);
        }
    }
}
