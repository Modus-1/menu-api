using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using menu_api.Repositories;
using menu_api.Models;
using menu_api.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using FluentAssertions;

namespace menu_api.Tests
{
    public class MenuItemRepositoryTests : IDisposable
    {
        private readonly MenuItemRepository _repository;
        private readonly MenuContext _context;

        public MenuItemRepositoryTests()
        {
            var options =
                new DbContextOptionsBuilder<MenuContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new MenuContext(options);
            _repository = new MenuItemRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task GetMenuItems_WithPopulatedTable_ShouldReturnAllMenuItems()
        {
            //arrange
            await _repository.InsertMenuItem(new MenuItem());
            await _repository.InsertMenuItem(new MenuItem());
            await _repository.InsertMenuItem(new MenuItem());

            //act
            var results = await _repository.GetMenuItems();

            //assert
            results
                .Should()
                .NotBeNull()
                .And.NotBeEmpty()
                .And.HaveCount(3);
        }

        [Fact]
        public async Task GetMenuItemById_WithPopulatedTable_ShouldReturnOneMenuItem()
        {
            //arrange
            Guid id = Guid.NewGuid();
            await _repository.InsertMenuItem(new MenuItem { Id = id });

            //act
            var result = await _repository.GetMenuItemByID(id);

            //assert
            result
                .Should()
                .NotBeNull()
                .And.BeEquivalentTo(new MenuItem { Id = id });
        }

        [Fact]
        public async Task GetMenuItemById_WithoutPopulatedTable_ShouldReturnNull()
        {
            //arrange
            Guid id = Guid.NewGuid();

            //act
            var result = await _repository.GetMenuItemByID(id);

            //assert
            result
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task InsertMenuItem_ShouldPopulateTable_WithTheMenuItem()
        {
            //arrange
            var menuItem = new MenuItem {
                Id = Guid.NewGuid(),
                Name = "erikse-kaas-souffle",
                IconUrl = "Https://Minecraft.net/",
                BannerUrl = "Https://Minecraft.net/",
                LongDescription = "lang",
                ShortDescription = "kort",
                Price = 34.90,
                CategoryId = 3
            };

            //act
            await _repository.InsertMenuItem(menuItem);
            var result = await _repository.GetMenuItemByID(menuItem.Id);

            //assert
            result
                .Should()
                .NotBeNull()
                .And.BeEquivalentTo(menuItem);
        }

        [Fact]
        public async Task InsertMenuItem_ThatAlreadyExistsInTable_ShouldThrowCorrectException()
        {
            //arrange
            var menuItem = new MenuItem
            {
                Id = Guid.NewGuid(),
                Name = "erikse-kaas-souffle",
                IconUrl = "Https://Minecraft.net/",
                BannerUrl = "Https://Minecraft.net/",
                LongDescription = "lang",
                ShortDescription = "kort",
                Price = 34.90,
                CategoryId = 3
            };

            //act
            await _repository.InsertMenuItem(menuItem);


            //assert
            await Assert.ThrowsAsync<ItemAlreadyExsits>(async () => await _repository.InsertMenuItem(menuItem));
        }
    }
}
