using menu_api.Models;

namespace menu_api.Repositories.Interfaces;

public interface ICategoryRepository
{

    Task<IEnumerable<Category>> GetAllCategories();

}