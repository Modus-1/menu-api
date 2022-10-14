using FluentAssertions;
using menu_api.Context;
using menu_api.Models;
using menu_api.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;
using menu_api.Exeptions;

namespace menu_api.Tests.RepositoryTests
{
    public class IngredientRepositoryTests : IDisposable
    {
        private readonly IngredientRepository _repository;
        private readonly MenuContext _context;

        public IngredientRepositoryTests()
        {
            var options =
                new DbContextOptionsBuilder<MenuContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new MenuContext(options);
            _repository = new IngredientRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task GetIngredients_WithPopulatedTable_ShouldReturnAllIngredients()
        {
            //arrange
            await _repository.InsertIngredient(new Ingredient());
            await _repository.InsertIngredient(new Ingredient());
            await _repository.InsertIngredient(new Ingredient());

            //act
            var results = await _repository.GetAllIngredients();

            //assert
            results
                .Should()
                .NotBeNull()
                .And.NotBeEmpty()
                .And.HaveCount(3);
        }


        [Fact]
        public async Task GetIngredientByID_WithPopulatedTable_ShouldReturnOneIngredient()
        {
            //arrange
            Guid id = Guid.NewGuid();
            await _repository.InsertIngredient(new Ingredient { Id = id });

            //act
            var result = await _repository.GetIngredientByID(id);

            //assert
            result
                .Should()
                .NotBeNull()
                .And.BeEquivalentTo(new Ingredient { Id = id });
        }

        [Fact]
        public async Task GetIngredientByID_WithoutPopulatedTable_ShouldReturnNull()
        {
            //arrange
            Guid id = Guid.NewGuid();

            //act
            var result = await _repository.GetIngredientByID(id);

            //assert
            result
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task InsertIngredient_ShouldPopulateTable_WithTheIngredient()
        {
            //arrange
            var ingredient = new Ingredient
            {
                Id = Guid.NewGuid(),
                Name = "erikse-kaas",
                Stock = 10,
                Allergens = "melkies ofz"
            };

            //act
            await _repository.InsertIngredient(ingredient);
            var result = await _repository.GetIngredientByID(ingredient.Id);

            //assert
            result
                .Should()
                .NotBeNull()
                .And.BeEquivalentTo(ingredient);
        }


        [Fact]
        public async Task InsertIngredient_ThatAlreadyExistsInTable_ShouldThrowItemAlreadyExistsException()
        {
            //arrange
            var ingredient = new Ingredient
            {
                Id = Guid.NewGuid(),
                Name = "erikse-kaas",
                Stock = 10,
                Allergens = "melkies ofz"
            };

            //act
            await _repository.InsertIngredient(ingredient);

            //assert
            await Assert.ThrowsAsync<ItemAlreadyExsistsExeption>(async () => await _repository.InsertIngredient(ingredient));
        }

        [Fact]
        public async Task DeleteIngredient_ShouldDelete_PopulatedTable()
        {
            //arrange
            var ingredient = new Ingredient
            {
                Id = Guid.NewGuid(),
                Name = "erikse-kaas",
                Stock = 10,
                Allergens = "melkies ofz"
            };

            var ingredient2 = new Ingredient
            {
                Id = Guid.NewGuid(),
                Name = "erikse-kaas2",
                Stock = 10,
                Allergens = "melkies ofz"
            };

            await _repository.InsertIngredient(ingredient);
            await _repository.InsertIngredient(ingredient2);

            //act
            await _repository.DeleteIngredient(ingredient.Id);

            var newIngredient = await _repository.GetIngredientByID(ingredient.Id); //Should return null because it was previously deleted
            var newIngredient2 = await _repository.GetIngredientByID(ingredient2.Id); //Should return newIngredient2 because it was not deleted

            //assert
            Assert.Null(newIngredient);
            Assert.NotNull(newIngredient2);
        }



        [Fact]
        public async Task UpdateIngredient_ShouldUpdatePopulatedTable()
        {
            //arrange
            var ingredient = new Ingredient
            {
                Id = Guid.NewGuid(),
                Name = "erikse-kaas",
                Stock = 10,
                Weight = 9001,
                Allergens = "melkies ofz"
            };

            var OldIngredientCopy = new Ingredient
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Stock = ingredient.Stock,
                Allergens = ingredient.Allergens
            };

            await _repository.InsertIngredient(ingredient);

            //act
            ingredient.Name = "boterham met kaas";
            ingredient.Stock = 8;
            ingredient.Allergens = "niks ofz";

            await _repository.UpdateIngredient(ingredient);
            var updatedMenuItem = await _repository.GetIngredientByID(ingredient.Id);


            //asserts
            updatedMenuItem.Should()
                .NotBeNull()
                .And.NotBeEquivalentTo(OldIngredientCopy)
                .And.BeEquivalentTo(ingredient);
        }
    }
}
