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
            menuItemRepository = new MenuItemRepository(menuContext);
        }

        [HttpGet]
        public IEnumerable<MenuItem> GetMenuItems()
        {
            return menuItemRepository.GetMenuItems();
        }

        [HttpGet("{id}")]
        public MenuItem GetMenuItemByID(Guid id)
        {
            return menuItemRepository.GetMenuItemByID(id);
        }

        [HttpPost]
        public void InsertMenuItem(MenuItem menuItem)
        {
            menuItemRepository.InsertMenuItem(menuItem);
            menuItemRepository.Save();
        }

    }
}
