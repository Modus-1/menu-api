using FluentAssertions;
using menu_api.Context;
using menu_api.Exeptions;
using menu_api.Models;
using menu_api.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace menu_api.Tests.RepositoryTests
{
    public class MenuItem_IngredientRepositoryTests : IDisposable
    {
        private readonly MenuItemIngredientRepository _repository;
        private readonly MenuContext _context;

        public MenuItem_IngredientRepositoryTests()
        {
            var options =
                new DbContextOptionsBuilder<MenuContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new MenuContext(options);
            _repository = new MenuItemIngredientRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            GC.SuppressFinalize(this);
        }


        [Fact]
        public async Task AddIngredientToMenuItem_ShouldPopulateMenuItem_IngredientTable_WithTheIngredient()
        {
            //Arrange
            var menuItem_Ingredient = new MenuItemIngredient
            {
                MenuItemId = Guid.NewGuid(),
                IngredientId = Guid.NewGuid(),
                Amount = 10
            };
            _context.MenuItems.Add(new MenuItem() { Id = menuItem_Ingredient.MenuItemId });
            _context.Ingredients.Add(new Ingredient() { Id = menuItem_Ingredient.IngredientId });
            //Act
            await _repository.AddIngredient(menuItem_Ingredient);
            var result = await _context.MenuItem_Ingredients.FindAsync(menuItem_Ingredient.MenuItemId, menuItem_Ingredient.IngredientId);

            //Assert
            result
                .Should()
                .NotBeNull()
                .And.BeEquivalentTo(menuItem_Ingredient);
        }


        [Fact]
        public async Task AddIngredientToMenuItem_ThatAlreadyExistsInTable_ShouldThrowCorrectException()
        {
            //Arrange
            var menuItem_Ingredient = new MenuItemIngredient
            {
                MenuItemId = Guid.NewGuid(),
                IngredientId = Guid.NewGuid(),
                Weight = 10
            };

            _context.MenuItems.Add(new MenuItem() { Id = menuItem_Ingredient.MenuItemId });
            _context.Ingredients.Add(new Ingredient() { Id = menuItem_Ingredient.IngredientId });

            //Act
            await _repository.AddIngredient(menuItem_Ingredient);

            //Assert
            await Assert.ThrowsAsync<ItemAlreadyExsistsException>(async () => await _repository.AddIngredient(menuItem_Ingredient));
        }

        [Fact]
        public async Task AddIngredientToMenuItem_WithNonExistingMenuItem_ShouldThrowCorrectException()
        {
            //Arrange
            var menuItem_Ingredient = new MenuItemIngredient
            {
                MenuItemId = Guid.NewGuid(),
                IngredientId = Guid.NewGuid(),
                Weight = 10
            };

            _context.Ingredients.Add(new Ingredient() { Id = menuItem_Ingredient.IngredientId });

            //Act
            var function = async () => await _repository.AddIngredient(menuItem_Ingredient);

            //Assert
            ItemDoesNotExistException ex = await Assert.ThrowsAsync<ItemDoesNotExistException>(function);
            Assert.Matches("MenuItem", ex.Message);
        }

        [Fact]
        public async Task AddIngredientToMenuItem_WithNonExistingIngredent_ShouldThrowCorrectException()
        {
            //Arrange
            var menuItem_Ingredient = new MenuItemIngredient
            {
                MenuItemId = Guid.NewGuid(),
                IngredientId = Guid.NewGuid(),
                Weight = 10
            };

            _context.MenuItems.Add(new MenuItem() { Id = menuItem_Ingredient.MenuItemId });

            //Act
            var function = async () => await _repository.AddIngredient(menuItem_Ingredient);

            //Assert
            ItemDoesNotExistException ex = await Assert.ThrowsAsync<ItemDoesNotExistException>(function);
            Assert.Matches("Ingredient", ex.Message);
        }

        [Fact]
        public async Task RemoveIngredientFromMenuItem_Ingredient_ShouldRemoveIngredient()
        {
            //Arrange
            var menuItem_Ingredient = new MenuItemIngredient
            {
                MenuItemId = Guid.NewGuid(),
                IngredientId = Guid.NewGuid(),
                Amount = 10
            };
            var menuItem_Ingredient2 = new MenuItemIngredient
            {
                MenuItemId = Guid.NewGuid(),
                IngredientId = Guid.NewGuid(),
                Amount = 10
            };
            _context.MenuItems.Add(new MenuItem() { Id = menuItem_Ingredient.MenuItemId });
            _context.Ingredients.Add(new Ingredient() { Id = menuItem_Ingredient.IngredientId });

            _context.MenuItems.Add(new MenuItem() { Id = menuItem_Ingredient2.MenuItemId });
            _context.Ingredients.Add(new Ingredient() { Id = menuItem_Ingredient2.IngredientId });

            await _repository.AddIngredient(menuItem_Ingredient);
            await _repository.AddIngredient(menuItem_Ingredient2);

            //Act

            await _repository.RemoveIngredient(menuItem_Ingredient.MenuItemId, menuItem_Ingredient.IngredientId);

            var result = await _context.MenuItem_Ingredients.FindAsync(menuItem_Ingredient.MenuItemId, menuItem_Ingredient.IngredientId);
            var result2 = await _context.MenuItem_Ingredients.FindAsync(menuItem_Ingredient2.MenuItemId, menuItem_Ingredient2.IngredientId);

            //Assert
            Assert.Null(result);
            Assert.NotNull(result2);
            result2.Should().BeEquivalentTo(menuItem_Ingredient2);
        }

        [Fact]
        public async Task RemoveIngredientFromMenuItem_ThatDoesNotExist_ShouldThrowItemDoesNotExistExeption()
        {
            //Arrange
            var menuItem_Ingredient = new MenuItemIngredient
            {
                MenuItemId = Guid.NewGuid(),
                IngredientId = Guid.NewGuid(),
                Amount = 10
            };


            //Act
            var function = async () => await _repository.RemoveIngredient(menuItem_Ingredient.MenuItemId, menuItem_Ingredient.IngredientId);

            //Assert
            await Assert.ThrowsAsync<ItemDoesNotExistException>(function);
        }

        [Fact]
        public async Task RemoveAllIngredientsFromMenuItem()
        {
            //Arrange
            var menuItemId = Guid.NewGuid();

            var menuItem_Ingredient = new MenuItemIngredient
            {
                MenuItemId = menuItemId,
                IngredientId = Guid.NewGuid(),
                Amount = 10
            };
            var menuItem_Ingredient2 = new MenuItemIngredient
            {
                MenuItemId = menuItemId,
                IngredientId = Guid.NewGuid(),
                Amount = 12
            };
            var menuItem_Ingredient3 = new MenuItemIngredient
            {
                MenuItemId = Guid.NewGuid(),
                IngredientId = Guid.NewGuid(),
                Amount = 12,
                Weight = 1990
            };
            _context.MenuItems.Add(new MenuItem() { Id = menuItem_Ingredient.MenuItemId });
            _context.Ingredients.Add(new Ingredient() { Id = menuItem_Ingredient.IngredientId });

            _context.Ingredients.Add(new Ingredient() { Id = menuItem_Ingredient2.IngredientId });

            _context.MenuItems.Add(new MenuItem() { Id = menuItem_Ingredient3.MenuItemId });
            _context.Ingredients.Add(new Ingredient() { Id = menuItem_Ingredient3.IngredientId });

            await _repository.AddIngredient(menuItem_Ingredient);
            await _repository.AddIngredient(menuItem_Ingredient2);
            await _repository.AddIngredient(menuItem_Ingredient3);

            //Act

            await _repository.RemoveAllIngredients(menuItemId);

            var result = await _context.MenuItem_Ingredients.FindAsync(menuItem_Ingredient.MenuItemId, menuItem_Ingredient.IngredientId);
            var result2 = await _context.MenuItem_Ingredients.FindAsync(menuItem_Ingredient2.MenuItemId, menuItem_Ingredient2.IngredientId);
            var result3 = await _context.MenuItem_Ingredients.FindAsync(menuItem_Ingredient3.MenuItemId, menuItem_Ingredient3.IngredientId);

            //Assert
            Assert.Null(result);
            Assert.Null(result2);
            Assert.NotNull(result3);
            result3.Should().BeEquivalentTo(menuItem_Ingredient3);
            Assert.Equal(menuItem_Ingredient3.Weight, result3.Weight);
            Assert.Equal(menuItem_Ingredient3.Amount, result3.Amount);
        }


        [Fact]
        public async Task RemoveAllIngredientsFromMenuItem_FromMenuItemThatDoesNotExist_ShouldThrowItemDoesNotExistExeption()
        {
            //Arrange
            var menuItem_Ingredient = new MenuItemIngredient
            {
                MenuItemId = Guid.NewGuid(),
                IngredientId = Guid.NewGuid(),
                Amount = 10
            };


            //Act
            var function = async () => await _repository.RemoveAllIngredients(menuItem_Ingredient.MenuItemId);

            //Assert
            ItemDoesNotExistException ex = await Assert.ThrowsAsync<ItemDoesNotExistException>(function);
            Assert.Matches("MenuItem", ex.Message);
        }

    }
}
