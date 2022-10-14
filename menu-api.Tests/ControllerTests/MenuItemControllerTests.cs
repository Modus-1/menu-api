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
            Assert.Equal(4, result.Count());
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

            var NotFoundResult = result.Result as NotFoundObjectResult;
            var OKResult = result.Result as OkResult;

            //assert
            NotFoundResult.Should().NotBeNull();
            OKResult.Should().BeNull();
        }

        [Fact]
        public async Task InsertMenuItem_ShouldReturnOK_WhenSuccessful()
        {
            //arrange
            MenuItem item = new MenuItem() { Id = Guid.NewGuid() };


            //act
            ActionResult result = await _controller.InsertMenuItem(item);

            var OKResult = result as OkResult;
            var ConflictResult = result as ConflictObjectResult;

            //assert
            OKResult.Should().NotBeNull();
            ConflictResult.Should().BeNull();
        }

        [Fact]
        public async Task InsertMenuItem_ShouldReturnConflict_WhenFailed()
        {
            //arrange
            MenuItem item = new MenuItem() { Id = Guid.NewGuid() };
            _MenuItemRepo.Setup(x => x.InsertMenuItem(item)).ThrowsAsync(new ItemAlreadyExsistsExeption());


            //act
            ActionResult result = await _controller.InsertMenuItem(item);

            var OKResult = result as OkResult;
            var ConflictResult = result as ConflictObjectResult;

            //assert
            OKResult.Should().BeNull();
            ConflictResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteMenuItem_ShouldReturnOK_WhenSuccessful()
        {
            //arrange
            MenuItem item = new MenuItem() { Id = Guid.NewGuid() };


            //act
            ActionResult result = await _controller.DeleteMenuItem(item.Id);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;

            //assert
            OKResult.Should().NotBeNull();
            NotFoundResult.Should().BeNull();
        }

        [Fact]
        public async Task DeleteMenuItem_ShouldReturnNotFound_WhenFailed()
        {
            //arrange
            MenuItem item = new MenuItem() { Id = Guid.NewGuid() };
            _MenuItemRepo.Setup(x => x.DeleteMenuItem(item.Id)).ThrowsAsync(new ItemDoesNotExistExeption());

            //act
            ActionResult result = await _controller.DeleteMenuItem(item.Id);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;

            //assert
            OKResult.Should().BeNull();
            NotFoundResult.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateMenuItem_ShouldReturnOK_WhenSuccessful()
        {
            //arrange
            MenuItem item = new MenuItem() { Id = Guid.NewGuid() };


            //act
            ActionResult result = await _controller.UpdateMenuItem(item);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;

            //assert
            OKResult.Should().NotBeNull();
            NotFoundResult.Should().BeNull();
        }

        [Fact]
        public async Task UpdateMenuItem_ShouldReturnNotFound_WhenFailed()
        {
            //arrange
            MenuItem item = new MenuItem() { Id = Guid.NewGuid() };
            _MenuItemRepo.Setup(x => x.UpdateMenuItem(item)).ThrowsAsync(new ItemDoesNotExistExeption());

            //act
            ActionResult result = await _controller.UpdateMenuItem(item);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;

            //assert
            OKResult.Should().BeNull();
            NotFoundResult.Should().NotBeNull();
        }

        [Fact]
        public async Task AddIngredientToMenuItem_ShouldReturnOK_WhenSuccessful()
        {
            //arrange
            MenuItem_Ingredient item = new MenuItem_Ingredient()
            {
                IngredientId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid()
            };

            //act
            ActionResult result = await _controller.AddIngredientToMenuItem(item);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;


            //assert
            OKResult.Should().NotBeNull();
            NotFoundResult.Should().BeNull();
        }

        [Fact]
        public async Task AddIngredientToMenuItem_ShouldReturnNotFound_WhenFailed()
        {
            //arrange
            MenuItem_Ingredient item = new MenuItem_Ingredient()
            {
                IngredientId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid()
            };
            _MenuItem_IngredientRepo.Setup(x => x.AddIngredient(item)).ThrowsAsync(new ItemDoesNotExistExeption());

            //act
            ActionResult result = await _controller.AddIngredientToMenuItem(item);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;


            //assert
            OKResult.Should().BeNull();
            NotFoundResult.Should().NotBeNull();
        }


        [Fact]
        public async Task DeleteIngredientFromMenuItem_ShouldReturnOK_WhenSuccessful()
        {
            //arrange
            MenuItem_Ingredient item = new MenuItem_Ingredient()
            {
                IngredientId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid()
            };

            //act
            ActionResult result = await _controller.DeleteIngredientFromMenuItem(item.MenuItemId, item.IngredientId);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;


            //assert
            OKResult.Should().NotBeNull();
            NotFoundResult.Should().BeNull();
        }

        [Fact]
        public async Task DeleteIngredientFromMenuItem_ShouldReturnNotFound_WhenFailed()
        {
            //arrange
            MenuItem_Ingredient item = new MenuItem_Ingredient()
            {
                IngredientId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid()
            };
            _MenuItem_IngredientRepo.Setup(x => x.RemoveIngredient(item.MenuItemId, item.IngredientId)).ThrowsAsync(new ItemDoesNotExistExeption());

            //act
            ActionResult result = await _controller.DeleteIngredientFromMenuItem(item.MenuItemId, item.IngredientId);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;


            //assert
            OKResult.Should().BeNull();
            NotFoundResult.Should().NotBeNull();
        }

    }
}
