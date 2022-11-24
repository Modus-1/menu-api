using FluentAssertions;
using menu_api.Context;
using menu_api.Models;
using menu_api.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using menu_api.Exceptions;
using Xunit;

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
            //Arrange
            await _repository.CreateIngredient(new Ingredient());
            await _repository.CreateIngredient(new Ingredient());
            await _repository.CreateIngredient(new Ingredient());

            //Act
            var results = await _repository.GetAllIngredients();

            //Assert
            results
                .Should()
                .NotBeNull()
                .And.NotBeEmpty()
                .And.HaveCount(3);
        }


        [Fact]
        public async Task GetIngredientByID_WithPopulatedTable_ShouldReturnOneIngredient()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            await _repository.CreateIngredient(new Ingredient { Id = id });

            //Act
            var result = await _repository.GetIngredientById(id);

            //Assert
            result
                .Should()
                .NotBeNull()
                .And.BeEquivalentTo(new Ingredient { Id = id });
        }

        [Fact]
        public async Task GetIngredientByID_WithoutPopulatedTable_ShouldReturnNull()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            //Act
            var result = await _repository.GetIngredientById(id);

            //Assert
            result
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task InsertIngredient_ShouldPopulateTable_WithTheIngredient()
        {
            //Arrange
            var ingredient = new Ingredient
            {
                Id = Guid.NewGuid(),
                Name = "erikse-kaas",
                Stock = 10,
                Allergens = "melkies ofz"
            };

            //Act
            await _repository.CreateIngredient(ingredient);
            var result = await _repository.GetIngredientById(ingredient.Id);

            //Assert
            result
                .Should()
                .NotBeNull()
                .And.BeEquivalentTo(ingredient);
        }


        [Fact]
        public async Task InsertIngredient_ThatAlreadyExistsInTable_ShouldThrowItemAlreadyExistsException()
        {
            //Arrange
            var ingredient = new Ingredient
            {
                Id = Guid.NewGuid(),
                Name = "erikse-kaas",
                Stock = 10,
                Allergens = "melkies ofz"
            };

            //Act
            await _repository.CreateIngredient(ingredient);

            //Assert
            await Assert.ThrowsAsync<ItemAlreadyExistsException>(async () => await _repository.CreateIngredient(ingredient));
        }

        [Fact]
        public async Task DeleteIngredient_ShouldDelete_PopulatedTable()
        {
            //Arrange
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

            await _repository.CreateIngredient(ingredient);
            await _repository.CreateIngredient(ingredient2);

            //Act
            await _repository.DeleteIngredient(ingredient.Id);

            var newIngredient = await _repository.GetIngredientById(ingredient.Id); //Should return null because it was previously deleted
            var newIngredient2 = await _repository.GetIngredientById(ingredient2.Id); //Should return newIngredient2 because it was not deleted

            //Assert
            Assert.Null(newIngredient);
            Assert.NotNull(newIngredient2);
        }

        [Fact]
        public async Task DeleteIngredient_ThatDoesNotExist_ShouldThrowItemDoesNotExistExeption()
        {
            //Arrange
            var ingredient = new Ingredient
            {
                Id = Guid.NewGuid(),
                Name = "erikse-kaas",
                Stock = 10,
                Allergens = "melkies ofz"
            };

            //Act
            var function = async () => await _repository.DeleteIngredient(ingredient.Id);

            //Assert
            await Assert.ThrowsAsync<ItemDoesNotExistException>(function);
        }



        [Fact]
        public async Task UpdateIngredient_ShouldUpdatePopulatedTable()
        {
            //Arrange
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

            await _repository.CreateIngredient(ingredient);

            //Act
            ingredient.Name = "boterham met kaas";
            ingredient.Stock = 8;
            ingredient.Allergens = "niks ofz";

            await _repository.UpdateIngredient(ingredient);
            var updatedMenuItem = await _repository.GetIngredientById(ingredient.Id);


            //Asserts
            updatedMenuItem.Should()
                .NotBeNull()
                .And.NotBeEquivalentTo(OldIngredientCopy)
                .And.BeEquivalentTo(ingredient);
        }

        [Fact]
        public async Task UpdateIngredient_ThatDoesNotExist_ShouldThrowItemDoesNotExistExeption()
        {
            //Arrange
            var ingredient = new Ingredient
            {
                Id = Guid.NewGuid(),
                Name = "erikse-kaas",
                Stock = 10,
                Allergens = "melkies ofz"
            };

            //Act
            var function = async () => await _repository.UpdateIngredient(ingredient);

            //Assert
            await Assert.ThrowsAsync<ItemDoesNotExistException>(function);
        }
    }
}
