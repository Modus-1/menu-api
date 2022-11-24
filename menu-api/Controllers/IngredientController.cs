using Microsoft.AspNetCore.Mvc;
using menu_api.Models;
using menu_api.Repositories;
using menu_api.Context;
using menu_api.Exeptions;
using menu_api.Repositories.Interfaces;

namespace menu_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientRepository _ingredientRepository;

        public IngredientController(IIngredientRepository ingredientRepo)
        {
            _ingredientRepository = ingredientRepo;
        }

        /// <summary>
        /// Gets all ingredients from the datastore.
        /// </summary>
        /// <returns>A list of all ingredients.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IEnumerable<Ingredient>> GetAllIngredients()
        {
            return await _ingredientRepository.GetAllIngredients();
        }

        /// <summary>
        /// Gets a single ingredient by its GUID.
        /// </summary>
        /// <param name="id">The GUID associated with the ingredient.</param>
        /// <returns>The ingredient found.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Ingredient>> GetIngredientById(Guid id)
        {
            var ingredient = await _ingredientRepository.GetIngredientById(id);
            if (ingredient == null)
            {
                return NotFound("Ingredient not found");
            }
            return ingredient;
        }

        /// <summary>
        /// Creates a new ingredient in the datastore.
        /// </summary>
        /// <param name="ingredient">The ingredient that is added.</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost]
        public async Task<ActionResult> CreateIngredient(Ingredient ingredient)
        {
            try
            {
                await _ingredientRepository.CreateIngredient(ingredient);
                return Ok();
            }
            catch (ItemAlreadyExsistsException)
            {
                return Conflict("Ingredient already exists");
            }
        }

        /// <summary>
        /// Deletes an ingredient from the datastore.
        /// </summary>
        /// <param name="id">The id of the ingredient to delete.</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:guid}")]
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

        /// <summary>
        /// Updates the information related to an ingredient.
        /// </summary>
        /// <param name="ingredient">The new ingredient to replace the previous ingredient with the same id.</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
