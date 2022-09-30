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


        public MenuItemController(MenuContext menuContext)
        {
            this.menuItemRepository = new MenuItemRepository(menuContext);

        }

        [HttpGet]
        public IEnumerable<MenuItem> GetMenuItems()
        {
            return this.menuItemRepository.GetMenuItems();
        }

    }
}
