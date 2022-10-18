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
    public class IngredientControllerTests
    {
        private readonly Mock<IIngredientRepository> _ingredientRepo;
        private readonly IngredientController _controller;

        public IngredientControllerTests()
        {
            _ingredientRepo = new Mock<IIngredientRepository>();
            _controller = new IngredientController(_ingredientRepo.Object);
        }

        [Fact]
        public async Task GetAllIngredients_ReturnsIngredientList()
        {
            //Arrange
            IEnumerable<Ingredient> Ingredients = new List<Ingredient>() { new Ingredient(), new Ingredient(), new Ingredient(), new Ingredient() };

            _ingredientRepo.Setup(x => x.GetAllIngredients()).ReturnsAsync(Ingredients);

            //Act
            var result = await _controller.GetAllIngredients();

            //Assert
            result.Should()
                .NotBeNull()
                .And.BeOfType<List<Ingredient>>();
            Assert.Equal(4, result.Count());
        }


        [Fact]
        public async Task GetIngredientByID_ReturnsCorrectIngredient()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Ingredient item = new Ingredient() { Id = id };
            Ingredient item2 = new Ingredient() { Id = id2 };


            _ingredientRepo.Setup(x => x.GetIngredientByID(id)).ReturnsAsync(item);
            _ingredientRepo.Setup(x => x.GetIngredientByID(id2)).ReturnsAsync(item2);

            //Act
            ActionResult<Ingredient> result = await _controller.GetIngredientByID(id);

            //Assert
            result.Value.Should()
                .NotBeNull()
                .And.BeEquivalentTo(item)
                .And.NotBeEquivalentTo(item2);
        }

        [Fact]
        public async Task GetIngredientByID_ShouldReturnNull_IfTableIsEmpty()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            Ingredient? item = null;
            _ingredientRepo.Setup(x => x.GetIngredientByID(id)).ReturnsAsync(item);

            //Act
            ActionResult<Ingredient> result = await _controller.GetIngredientByID(id);

            var NotFoundResult = result.Result as NotFoundObjectResult;
            var OKResult = result.Result as OkResult;

            //Assert
            NotFoundResult.Should().NotBeNull();
            OKResult.Should().BeNull();
        }

        [Fact]
        public async Task InsertIngredient_ShouldReturnOK_WhenSuccessful()
        {
            //Arrange
            Ingredient item = new Ingredient() { Id = Guid.NewGuid() };


            //Act
            ActionResult result = await _controller.InsertIngredient(item);

            var OKResult = result as OkResult;
            var ConflictResult = result as ConflictObjectResult;

            //Assert
            OKResult.Should().NotBeNull();
            ConflictResult.Should().BeNull();
        }

        [Fact]
        public async Task InsertIngredient_ShouldReturnConflict_WhenFailed()
        {
            //Arrange
            Ingredient item = new Ingredient() { Id = Guid.NewGuid() };
            _ingredientRepo.Setup(x => x.InsertIngredient(item)).ThrowsAsync(new ItemAlreadyExsistsException());


            //Act
            ActionResult result = await _controller.InsertIngredient(item);

            var OKResult = result as OkResult;
            var ConflictResult = result as ConflictObjectResult;

            //Assert
            OKResult.Should().BeNull();
            ConflictResult.Should().NotBeNull();
        }


        [Fact]
        public async Task DeleteIngredient_ShouldReturnOK_WhenSuccessful()
        {
            //Arrange
            Ingredient item = new Ingredient() { Id = Guid.NewGuid() };


            //Act
            ActionResult result = await _controller.DeleteIngredient(item.Id);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;

            //Assert
            OKResult.Should().NotBeNull();
            NotFoundResult.Should().BeNull();
        }

        [Fact]
        public async Task DeleteIngredient_ShouldReturnNotFound_WhenFailed()
        {
            //Arrange
            Ingredient item = new Ingredient() { Id = Guid.NewGuid() };
            _ingredientRepo.Setup(x => x.DeleteIngredient(item.Id)).ThrowsAsync(new ItemDoesNotExistException());

            //Act
            ActionResult result = await _controller.DeleteIngredient(item.Id);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;

            //Assert
            OKResult.Should().BeNull();
            NotFoundResult.Should().NotBeNull();
        }


        [Fact]
        public async Task UpdateIngredient_ShouldReturnOK_WhenSuccessful()
        {
            //Arrange
            Ingredient item = new Ingredient() { Id = Guid.NewGuid() };


            //Act
            ActionResult result = await _controller.UpdateIngredient(item);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;

            //Assert
            OKResult.Should().NotBeNull();
            NotFoundResult.Should().BeNull();
        }

        [Fact]
        public async Task UpdateIngredient_ShouldReturnNotFound_WhenFailed()
        {
            //Arrange
            Ingredient item = new Ingredient() { Id = Guid.NewGuid() };
            _ingredientRepo.Setup(x => x.UpdateIngredient(item)).ThrowsAsync(new ItemDoesNotExistException());

            //Act
            ActionResult result = await _controller.UpdateIngredient(item);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;

            //Assert
            OKResult.Should().BeNull();
            NotFoundResult.Should().NotBeNull();
        }
    }
}
