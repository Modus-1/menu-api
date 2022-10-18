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
using menu_api.Exeptions;

namespace menu_api.Tests.ControllerTests
{
    public class MenuItemControllerTests
    {
        private readonly Mock<IMenuItemRepository> _menuItemRepo;
        private readonly Mock<IMenuItemIngredientRepository> _menuItem_ingredientRepo;
        private readonly MenuItemController _controller;

        public MenuItemControllerTests()
        {
            _menuItemRepo = new Mock<IMenuItemRepository>();
            _menuItem_ingredientRepo = new Mock<IMenuItemIngredientRepository>();

            _controller = new MenuItemController(_menuItemRepo.Object, _menuItem_ingredientRepo.Object);
        }

        [Fact]
        public async Task GetMenuItems_ReturnsMenuItemList()
        {
            //Arrange
            IEnumerable<MenuItem> items = new List<MenuItem>() { new MenuItem(), new MenuItem(), new MenuItem(), new MenuItem() };

            _menuItemRepo.Setup(x => x.GetMenuItems()).ReturnsAsync(items);

            //Act
            var result = await _controller.GetMenuItems();

            //Assert
            result.Should()
                .NotBeNull()
                .And.BeOfType<List<MenuItem>>();
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async Task GetMenuItemById_ReturnsCorrectMenuItem()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            MenuItem item = new MenuItem() { Id = id };
            MenuItem item2 = new MenuItem() { Id = id2 };


            _menuItemRepo.Setup(x => x.GetMenuItemByID(id)).ReturnsAsync(item);
            _menuItemRepo.Setup(x => x.GetMenuItemByID(id2)).ReturnsAsync(item2);

            //Act
            ActionResult<MenuItem> result = await _controller.GetMenuItemByID(id);

            //Assert
            result.Value.Should()
                .NotBeNull()
                .And.BeEquivalentTo(item)
                .And.NotBeEquivalentTo(item2);
        }

        [Fact]
        public async Task GetMenuItemById_ShouldReturnNull_IfTableIsEmpty()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            MenuItem? item = null;
            _menuItemRepo.Setup(x => x.GetMenuItemByID(id)).ReturnsAsync(item);

            //Act
            ActionResult<MenuItem> result = await _controller.GetMenuItemByID(id);

            var NotFoundResult = result.Result as NotFoundObjectResult;
            var OKResult = result.Result as OkResult;

            //Assert
            NotFoundResult.Should().NotBeNull();
            OKResult.Should().BeNull();
        }

        [Fact]
        public async Task InsertMenuItem_ShouldReturnOK_WhenSuccessful()
        {
            //Arrange
            MenuItem item = new MenuItem() { Id = Guid.NewGuid() };


            //Act
            ActionResult result = await _controller.InsertMenuItem(item);

            var OKResult = result as OkResult;
            var ConflictResult = result as ConflictObjectResult;

            //Assert
            OKResult.Should().NotBeNull();
            ConflictResult.Should().BeNull();
        }

        [Fact]
        public async Task InsertMenuItem_ShouldReturnConflict_WhenFailed()
        {
            //Arrange
            MenuItem item = new MenuItem() { Id = Guid.NewGuid() };
            _menuItemRepo.Setup(x => x.InsertMenuItem(item)).ThrowsAsync(new ItemAlreadyExsistsException());


            //Act
            ActionResult result = await _controller.InsertMenuItem(item);

            var OKResult = result as OkResult;
            var ConflictResult = result as ConflictObjectResult;

            //Assert
            OKResult.Should().BeNull();
            ConflictResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteMenuItem_ShouldReturnOK_WhenSuccessful()
        {
            //Arrange
            MenuItem item = new MenuItem() { Id = Guid.NewGuid() };


            //Act
            ActionResult result = await _controller.DeleteMenuItem(item.Id);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;

            //Assert
            OKResult.Should().NotBeNull();
            NotFoundResult.Should().BeNull();
        }

        [Fact]
        public async Task DeleteMenuItem_ShouldReturnNotFound_WhenFailed()
        {
            //Arrange
            MenuItem item = new MenuItem() { Id = Guid.NewGuid() };
            _menuItemRepo.Setup(x => x.DeleteMenuItem(item.Id)).ThrowsAsync(new ItemDoesNotExistException());

            //Act
            ActionResult result = await _controller.DeleteMenuItem(item.Id);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;

            //Assert
            OKResult.Should().BeNull();
            NotFoundResult.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateMenuItem_ShouldReturnOK_WhenSuccessful()
        {
            //Arrange
            MenuItem item = new MenuItem() { Id = Guid.NewGuid() };


            //Act
            ActionResult result = await _controller.UpdateMenuItem(item);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;

            //Assert
            OKResult.Should().NotBeNull();
            NotFoundResult.Should().BeNull();
        }

        [Fact]
        public async Task UpdateMenuItem_ShouldReturnNotFound_WhenFailed()
        {
            //Arrange
            MenuItem item = new MenuItem() { Id = Guid.NewGuid() };
            _menuItemRepo.Setup(x => x.UpdateMenuItem(item)).ThrowsAsync(new ItemDoesNotExistException());

            //Act
            ActionResult result = await _controller.UpdateMenuItem(item);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;

            //Assert
            OKResult.Should().BeNull();
            NotFoundResult.Should().NotBeNull();
        }

        [Fact]
        public async Task AddIngredientToMenuItem_ShouldReturnOK_WhenSuccessful()
        {
            //Arrange
            MenuItemIngredient item = new MenuItemIngredient()
            {
                IngredientId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid()
            };

            //Act
            ActionResult result = await _controller.AddIngredientToMenuItem(item);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;


            //Assert
            OKResult.Should().NotBeNull();
            NotFoundResult.Should().BeNull();
        }

        [Fact]
        public async Task AddIngredientToMenuItem_ShouldReturnNotFound_WhenFailed()
        {
            //Arrange
            MenuItemIngredient item = new MenuItemIngredient()
            {
                IngredientId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid()
            };
            _menuItem_ingredientRepo.Setup(x => x.AddIngredient(item)).ThrowsAsync(new ItemDoesNotExistException());

            //Act
            ActionResult result = await _controller.AddIngredientToMenuItem(item);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;


            //Assert
            OKResult.Should().BeNull();
            NotFoundResult.Should().NotBeNull();
        }


        [Fact]
        public async Task DeleteIngredientFromMenuItem_ShouldReturnOK_WhenSuccessful()
        {
            //Arrange
            MenuItemIngredient item = new MenuItemIngredient()
            {
                IngredientId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid()
            };

            //Act
            ActionResult result = await _controller.DeleteIngredientFromMenuItem(item.MenuItemId, item.IngredientId);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;


            //Assert
            OKResult.Should().NotBeNull();
            NotFoundResult.Should().BeNull();
        }

        [Fact]
        public async Task DeleteIngredientFromMenuItem_ShouldReturnNotFound_WhenFailed()
        {
            //Arrange
            MenuItemIngredient item = new MenuItemIngredient()
            {
                IngredientId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid()
            };
            _menuItem_ingredientRepo.Setup(x => x.RemoveIngredient(item.MenuItemId, item.IngredientId)).ThrowsAsync(new ItemDoesNotExistException());

            //Act
            ActionResult result = await _controller.DeleteIngredientFromMenuItem(item.MenuItemId, item.IngredientId);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;


            //Assert
            OKResult.Should().BeNull();
            NotFoundResult.Should().NotBeNull();
        }

    }
}
