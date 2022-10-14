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
        private readonly Mock<IIngredientRepository> _IngredientRepo;
        private readonly IngredientController _controller;

        public IngredientControllerTests()
        {
            _IngredientRepo = new Mock<IIngredientRepository>();
            _controller = new IngredientController(_IngredientRepo.Object);
        }

        [Fact]
        public async Task GetAllIngredients_ReturnsIngredientList()
        {
            //arrange
            IEnumerable<Ingredient> Ingredients = new List<Ingredient>() { new Ingredient(), new Ingredient(), new Ingredient(), new Ingredient() };

            _IngredientRepo.Setup(x => x.GetAllIngredients()).ReturnsAsync(Ingredients);

            //act
            var result = await _controller.GetAllIngredients();

            //assert
            result.Should()
                .NotBeNull()
                .And.BeOfType<List<Ingredient>>();
            Assert.Equal(4, result.Count());
        }


        [Fact]
        public async Task GetIngredientByID_ReturnsCorrectIngredient()
        {
            //arrange
            Guid id = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            Ingredient item = new Ingredient() { Id = id };
            Ingredient item2 = new Ingredient() { Id = id2 };


            _IngredientRepo.Setup(x => x.GetIngredientByID(id)).ReturnsAsync(item);
            _IngredientRepo.Setup(x => x.GetIngredientByID(id2)).ReturnsAsync(item2);

            //act
            ActionResult<Ingredient> result = await _controller.GetIngredientByID(id);

            //assert
            result.Value.Should()
                .NotBeNull()
                .And.BeEquivalentTo(item)
                .And.NotBeEquivalentTo(item2);
        }

        [Fact]
        public async Task GetIngredientByID_ShouldReturnNull_IfTableIsEmpty()
        {
            //arrange
            Guid id = Guid.NewGuid();
            Ingredient? item = null;
            _IngredientRepo.Setup(x => x.GetIngredientByID(id)).ReturnsAsync(item);

            //act
            ActionResult<Ingredient> result = await _controller.GetIngredientByID(id);

            var NotFoundResult = result.Result as NotFoundObjectResult;
            var OKResult = result.Result as OkResult;

            //assert
            NotFoundResult.Should().NotBeNull();
            OKResult.Should().BeNull();
        }

        [Fact]
        public async Task InsertIngredient_ShouldReturnOK_WhenSuccessful()
        {
            //arrange
            Ingredient item = new Ingredient() { Id = Guid.NewGuid() };


            //act
            ActionResult result = await _controller.InsertIngredient(item);

            var OKResult = result as OkResult;
            var ConflictResult = result as ConflictObjectResult;

            //assert
            OKResult.Should().NotBeNull();
            ConflictResult.Should().BeNull();
        }

        [Fact]
        public async Task InsertIngredient_ShouldReturnConflict_WhenFailed()
        {
            //arrange
            Ingredient item = new Ingredient() { Id = Guid.NewGuid() };
            _IngredientRepo.Setup(x => x.InsertIngredient(item)).ThrowsAsync(new ItemAlreadyExsistsExeption());


            //act
            ActionResult result = await _controller.InsertIngredient(item);

            var OKResult = result as OkResult;
            var ConflictResult = result as ConflictObjectResult;

            //assert
            OKResult.Should().BeNull();
            ConflictResult.Should().NotBeNull();
        }


        [Fact]
        public async Task DeleteIngredient_ShouldReturnOK_WhenSuccessful()
        {
            //arrange
            Ingredient item = new Ingredient() { Id = Guid.NewGuid() };


            //act
            ActionResult result = await _controller.DeleteIngredient(item.Id);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;

            //assert
            OKResult.Should().NotBeNull();
            NotFoundResult.Should().BeNull();
        }

        [Fact]
        public async Task DeleteIngredient_ShouldReturnNotFound_WhenFailed()
        {
            //arrange
            Ingredient item = new Ingredient() { Id = Guid.NewGuid() };
            _IngredientRepo.Setup(x => x.DeleteIngredient(item.Id)).ThrowsAsync(new ItemDoesNotExistExeption());

            //act
            ActionResult result = await _controller.DeleteIngredient(item.Id);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;

            //assert
            OKResult.Should().BeNull();
            NotFoundResult.Should().NotBeNull();
        }


        [Fact]
        public async Task UpdateIngredient_ShouldReturnOK_WhenSuccessful()
        {
            //arrange
            Ingredient item = new Ingredient() { Id = Guid.NewGuid() };


            //act
            ActionResult result = await _controller.UpdateIngredient(item);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;

            //assert
            OKResult.Should().NotBeNull();
            NotFoundResult.Should().BeNull();
        }

        [Fact]
        public async Task UpdateIngredient_ShouldReturnNotFound_WhenFailed()
        {
            //arrange
            Ingredient item = new Ingredient() { Id = Guid.NewGuid() };
            _IngredientRepo.Setup(x => x.UpdateIngredient(item)).ThrowsAsync(new ItemDoesNotExistExeption());

            //act
            ActionResult result = await _controller.UpdateIngredient(item);

            var OKResult = result as OkResult;
            var NotFoundResult = result as NotFoundObjectResult;

            //assert
            OKResult.Should().BeNull();
            NotFoundResult.Should().NotBeNull();
        }
    }
}
