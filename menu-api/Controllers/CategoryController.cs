using menu_api.Models;
using menu_api.Repositories;
using menu_api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace menu_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _repository;

    public CategoryController(ICategoryRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets all categories from the datastore.
    /// </summary>
    /// <returns>A list of all categories.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = (await _repository.GetAllCategories()).ToList();
        return Ok(result);
    }
}