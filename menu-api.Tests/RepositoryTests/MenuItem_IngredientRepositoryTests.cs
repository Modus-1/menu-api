using FluentAssertions;
using menu_api.Context;
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
        private readonly MenuItem_IngredientRepository _repository;
        private readonly MenuContext _context;

        public MenuItem_IngredientRepositoryTests()
        {
            var options =
                new DbContextOptionsBuilder<MenuContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new MenuContext(options);
            _repository = new MenuItem_IngredientRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            GC.SuppressFinalize(this);
        }


        [Fact]
        public async Task AddIngredientToMenuItem_ShouldPopulateMenuItem_IngredientTable_WithTheIngredient()
        {
            //arrange
            var menuItem_Ingredient = new MenuItem_Ingredient
            {
                MenuItemId = Guid.NewGuid(),
                IngredientId = Guid.NewGuid(),
                Amount = 10
            };
            _context.MenuItems.Add(new MenuItem() { Id = menuItem_Ingredient.MenuItemId });
            _context.Ingredients.Add(new Ingredient() { Id = menuItem_Ingredient.IngredientId });
            //act
            await _repository.AddIngredient(menuItem_Ingredient);
            var result = await _context.MenuItem_Ingredients.FindAsync(menuItem_Ingredient.MenuItemId, menuItem_Ingredient.IngredientId);

            //assert
            result
                .Should()
                .NotBeNull()
                .And.BeEquivalentTo(menuItem_Ingredient);
        }

        [Fact]
        public async Task RemoveIngredientFromMenuItem_Ingredient_ShouldRemoveIngredient()
        {
            //arrange
            var menuItem_Ingredient = new MenuItem_Ingredient
            {
                MenuItemId = Guid.NewGuid(),
                IngredientId = Guid.NewGuid(),
                Amount = 10
            };
            var menuItem_Ingredient2 = new MenuItem_Ingredient
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

            //act

            await _repository.RemoveIngredient(menuItem_Ingredient.MenuItemId, menuItem_Ingredient.IngredientId);

            var result = await _context.MenuItem_Ingredients.FindAsync(menuItem_Ingredient.MenuItemId, menuItem_Ingredient.IngredientId);
            var result2 = await _context.MenuItem_Ingredients.FindAsync(menuItem_Ingredient2.MenuItemId, menuItem_Ingredient2.IngredientId);

            //assert
            Assert.Null(result);
            Assert.NotNull(result2);
            result2.Should().BeEquivalentTo(menuItem_Ingredient2);
        }



        [Fact]
        public async Task RemoveAllIngredientsFromMenuItem()
        {
            //arrange
            var menuItemId = Guid.NewGuid();

            var menuItem_Ingredient = new MenuItem_Ingredient
            {
                MenuItemId = menuItemId,
                IngredientId = Guid.NewGuid(),
                Amount = 10
            };
            var menuItem_Ingredient2 = new MenuItem_Ingredient
            {
                MenuItemId = menuItemId,
                IngredientId = Guid.NewGuid(),
                Amount = 12
            };
            var menuItem_Ingredient3 = new MenuItem_Ingredient
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

            //act

            await _repository.RemoveAllIngredients(menuItemId);

            var result = await _context.MenuItem_Ingredients.FindAsync(menuItem_Ingredient.MenuItemId, menuItem_Ingredient.IngredientId);
            var result2 = await _context.MenuItem_Ingredients.FindAsync(menuItem_Ingredient2.MenuItemId, menuItem_Ingredient2.IngredientId);
            var result3 = await _context.MenuItem_Ingredients.FindAsync(menuItem_Ingredient3.MenuItemId, menuItem_Ingredient3.IngredientId);

            //assert
            Assert.Null(result);
            Assert.Null(result2);
            Assert.NotNull(result3);
            result3.Should().BeEquivalentTo(menuItem_Ingredient3);
        }
    }
}
