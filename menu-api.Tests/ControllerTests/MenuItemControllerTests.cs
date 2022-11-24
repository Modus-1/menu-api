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
using menu_api.Repositories.Interfaces;

namespace menu_api.Tests.ControllerTests
{
    public class MenuItemControllerTests
    {
        private readonly Mock<IMenuItemRepository> _menuItemRepo;
        private readonly Mock<IMenuItemIngredientRepository> _menuItemIngredientRepository;
        private readonly Mock<ICategoryRepository> _categoryRepository;
        private readonly MenuItemController _controller;

        public MenuItemControllerTests()
        {
            _menuItemRepo = new Mock<IMenuItemRepository>();
            _menuItemIngredientRepository = new Mock<IMenuItemIngredientRepository>();
            _categoryRepository = new Mock<ICategoryRepository>();

            _controller = new MenuItemController(
                _menuItemRepo.Object, 
                _menuItemIngredientRepository.Object, 
                _categoryRepository.Object
                );
        }

        [Fact]
        public async Task GetMenuItems_ReturnsMenuItemList()
        {
            //Arrange
            IEnumerable<MenuItem> items = new List<MenuItem>() { new(), new(), new(), new() };

            _menuItemRepo.Setup(x => x.GetMenuItems()).ReturnsAsync(items);

            //Act
            var result = await _controller.GetMenuItems();

            //Assert
            result.Should()
                .NotBeNull()
                .And.HaveCount(4);
        }

        [Fact]
        public async Task GetMenuItemById_ReturnsCorrectMenuItem()
        {
            //Arrange
            var id = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var item = new MenuItem() { Id = id };
            var item2 = new MenuItem() { Id = id2 };


            _menuItemRepo.Setup(x => x.GetMenuItemById(id)).ReturnsAsync(item);
            _menuItemRepo.Setup(x => x.GetMenuItemById(id2)).ReturnsAsync(item2);

            //Act
            var result = await _controller.GetMenuItemById(id);

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
            var id = Guid.NewGuid();
            MenuItem? item = null;
            _menuItemRepo.Setup(x => x.GetMenuItemById(id)).ReturnsAsync(item);

            //Act
            var result = await _controller.GetMenuItemById(id);

            var notFoundObjectResult = result.Result as NotFoundObjectResult;

            //Assert
            notFoundObjectResult.Should().NotBeNull();
        }

        [Fact]
        public async Task InsertMenuItem_ShouldReturnOK_WhenSuccessful()
        {
            //Arrange
            var categoryGuid = Guid.NewGuid();
            var item = new MenuItem() { Id = Guid.NewGuid(), CategoryId = categoryGuid};
            _categoryRepository.Setup(repository => repository.GetAllCategories()).ReturnsAsync(
                new List<Category> {new() {Id = categoryGuid}}
                );

            //Act
            var result = await _controller.CreateMenuItem(item);
            var okResult = result as OkResult;

            //Assert
            okResult.Should().NotBeNull();
        }

        [Fact]
        public async Task InsertMenuItem_ShouldReturnConflict_WhenFailed()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var item = new MenuItem() { Id = Guid.NewGuid(), CategoryId = guid};
            _menuItemRepo.Setup(x => x.CreateMenuItem(item)).ThrowsAsync(new ItemAlreadyExsistsException());
            _categoryRepository.Setup(repository => repository.GetAllCategories()).ReturnsAsync(
                new List<Category>{new() {Id = guid}}
            );

            //Act
            var result = await _controller.CreateMenuItem(item);
            var conflictResult = result as ConflictObjectResult;

            //Assert
            conflictResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteMenuItem_ShouldReturnOK_WhenSuccessful()
        {
            //Arrange
            var item = new MenuItem() { Id = Guid.NewGuid() };


            //Act
            var result = await _controller.DeleteMenuItem(item.Id);

            var okResult = result as OkResult;

            //Assert
            okResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteMenuItem_ShouldReturnNotFound_WhenFailed()
        {
            //Arrange
            var item = new MenuItem() { Id = Guid.NewGuid() };
            _menuItemRepo.Setup(x => x.DeleteMenuItem(item.Id)).ThrowsAsync(new ItemDoesNotExistException());

            //Act
            var result = await _controller.DeleteMenuItem(item.Id);

            var notFoundObjectResult = result as NotFoundObjectResult;

            //Assert
            notFoundObjectResult.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateMenuItem_ShouldReturnOK_WhenSuccessful()
        {
            //Arrange
            var item = new MenuItem() { Id = Guid.NewGuid() };


            //Act
            var result = await _controller.UpdateMenuItem(item);

            var okResult = result as OkResult;

            //Assert
            okResult.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateMenuItem_ShouldReturnNotFound_WhenFailed()
        {
            //Arrange
            var item = new MenuItem() { Id = Guid.NewGuid() };
            _menuItemRepo.Setup(x => x.UpdateMenuItem(item)).ThrowsAsync(new ItemDoesNotExistException());

            //Act
            var result = await _controller.UpdateMenuItem(item);

            var notFoundObjectResult = result as NotFoundObjectResult;

            //Assert
            notFoundObjectResult.Should().NotBeNull();
        }

        [Fact]
        public async Task AddIngredientToMenuItem_ShouldReturnOK_WhenSuccessful()
        {
            //Arrange
            var item = new MenuItemIngredient()
            {
                IngredientId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid()
            };

            //Act
            var result = await _controller.AddIngredientToMenuItem(item);
            var okResult = result as OkResult;


            //Assert
            okResult.Should().NotBeNull();
        }

        [Fact]
        public async Task AddIngredientToMenuItem_ShouldReturnNotFound_WhenFailed()
        {
            //Arrange
            var item = new MenuItemIngredient()
            {
                IngredientId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid()
            };
            _menuItemIngredientRepository.Setup(x => x.AddIngredient(item)).ThrowsAsync(new ItemDoesNotExistException());

            //Act
            var result = await _controller.AddIngredientToMenuItem(item);
            var notFoundObjectResult = result as NotFoundObjectResult;


            //Assert
            notFoundObjectResult.Should().NotBeNull();
        }


        [Fact]
        public async Task DeleteIngredientFromMenuItem_ShouldReturnOK_WhenSuccessful()
        {
            //Arrange
            var item = new MenuItemIngredient()
            {
                IngredientId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid()
            };

            //Act
            var result = await _controller.DeleteIngredientFromMenuItem(item.MenuItemId, item.IngredientId);

            var okResult = result as OkResult;


            //Assert
            okResult.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteIngredientFromMenuItem_ShouldReturnNotFound_WhenFailed()
        {
            //Arrange
            var item = new MenuItemIngredient()
            {
                IngredientId = Guid.NewGuid(),
                MenuItemId = Guid.NewGuid()
            };
            _menuItemIngredientRepository.Setup(x => x.RemoveIngredient(item.MenuItemId, item.IngredientId)).ThrowsAsync(new ItemDoesNotExistException());

            //Act
            var result = await _controller.DeleteIngredientFromMenuItem(item.MenuItemId, item.IngredientId);
            var notFoundObjectResult = result as NotFoundObjectResult;


            //Assert
            notFoundObjectResult.Should().NotBeNull();
        }

    }
}
