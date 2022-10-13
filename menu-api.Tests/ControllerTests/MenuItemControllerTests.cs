using FluentAssertions;
using menu_api.Context;
using menu_api.Controllers;
using menu_api.Models;
using menu_api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace menu_api.Tests.ControllerTests
{
    public class MenuItemControllerTests
    {
        private readonly Mock<IMenuItemRepository> _MenuItemRepo;
        private readonly Mock<IMenuItem_IngredientRepository> _MenuItem_IngredientRepo;
        private readonly MenuItemController _controller;

        public MenuItemControllerTests()
        {
            _MenuItemRepo = new Mock<IMenuItemRepository>();
            _MenuItem_IngredientRepo = new Mock<IMenuItem_IngredientRepository>();

            _controller = new MenuItemController(_MenuItemRepo.Object, _MenuItem_IngredientRepo.Object);
        }

        [Fact]
        public async Task GetMenuItems_ReturnsMenuItemList()
        {
            //arrange
            IEnumerable<MenuItem> items = new List<MenuItem>() { new MenuItem(), new MenuItem(), new MenuItem(), new MenuItem() };

            _MenuItemRepo.Setup(x => x.GetMenuItems()).ReturnsAsync(items);

            //act
            var result = await _controller.GetMenuItems();

            //assert
            result.Should()
                .NotBeNull()
                .And.BeOfType<List<MenuItem>>();
            Assert.Equal(4, items.Count());
        }

        [Fact]
        public async Task GetMenuItemById_ReturnsCorrectMenuItem()
        {
            //arrange
            Guid id = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            MenuItem item = new MenuItem() { Id = id };
            MenuItem item2 = new MenuItem() { Id = id2 };


            _MenuItemRepo.Setup(x => x.GetMenuItemByID(id)).ReturnsAsync(item);
            _MenuItemRepo.Setup(x => x.GetMenuItemByID(id2)).ReturnsAsync(item2);

            //act
            ActionResult<MenuItem> result = await _controller.GetMenuItemByID(id);

            //assert
            result.Value.Should()
                .NotBeNull()
                .And.BeEquivalentTo(item)
                .And.NotBeEquivalentTo(item2);
        }

        [Fact]
        public async Task GetMenuItemById_ShouldReturnNull_IfTableIsEmpty()
        {
            //arrange
            Guid id = Guid.NewGuid();
            MenuItem? item = null;
            _MenuItemRepo.Setup(x => x.GetMenuItemByID(id)).ReturnsAsync(item);

            //act
            ActionResult<MenuItem> result = await _controller.GetMenuItemByID(id);

            var NotFoundResult = result.Result as NotFoundResult;
            var OKResult = result.Result as OkResult;

            //assert
            NotFoundResult.Should().NotBeNull();
            OKResult.Should().BeNull();
        }
    }
}
