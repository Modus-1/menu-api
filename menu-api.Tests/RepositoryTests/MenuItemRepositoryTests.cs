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
            //Arrange
            await _repository.InsertMenuItem(new MenuItem());
            await _repository.InsertMenuItem(new MenuItem());
            await _repository.InsertMenuItem(new MenuItem());

            //Act
            var results = await _repository.GetMenuItems();

            //Assert
            results
                .Should()
                .NotBeNull()
                .And.NotBeEmpty()
                .And.HaveCount(3);
        }

        [Fact]
        public async Task GetMenuItemById_WithPopulatedTable_ShouldReturnOneMenuItem()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            await _repository.InsertMenuItem(new MenuItem { Id = id });

            //Act
            var result = await _repository.GetMenuItemByID(id);

            //Assert
            result
                .Should()
                .NotBeNull()
                .And.BeEquivalentTo(new MenuItem { Id = id });
        }

        [Fact]
        public async Task GetMenuItemById_WithoutPopulatedTable_ShouldReturnNull()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            //Act
            var result = await _repository.GetMenuItemByID(id);

            //Assert
            result
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task InsertMenuItem_ShouldPopulateTable_WithTheMenuItem()
        {
            //Arrange
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

            //Act
            await _repository.InsertMenuItem(menuItem);
            var result = await _repository.GetMenuItemByID(menuItem.Id);

            //Assert
            result
                .Should()
                .NotBeNull()
                .And.BeEquivalentTo(menuItem);
        }

        [Fact]
        public async Task InsertMenuItem_ThatAlreadyExistsInTable_ShouldThrowCorrectException()
        {
            //Arrange
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

            //Act
            await _repository.InsertMenuItem(menuItem);

            //Assert
            await Assert.ThrowsAsync<ItemAlreadyExsistsException>(async () => await _repository.InsertMenuItem(menuItem));
        }

        [Fact]
        public async Task DeleteMenuItem_ShouldDelete_PopulatedTable()
        {
            //Arrange
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

            var menuItem2 = new MenuItem
            {
                Id = Guid.NewGuid(),
                Name = "erikse-kaas-souffle2",
                IconUrl = "Https://Minecraft.net/",
                BannerUrl = "Https://Minecraft.net/",
                LongDescription = "lang",
                ShortDescription = "kort",
                Price = 34.90,
                CategoryId = 3
            };

            await _repository.InsertMenuItem(menuItem);
            await _repository.InsertMenuItem(menuItem2);

            //Act
            await _repository.DeleteMenuItem(menuItem.Id);

            var newMenuItem = await _repository.GetMenuItemByID(menuItem.Id); //Should return null because it was previously deleted
            var newMenuItem2 = await _repository.GetMenuItemByID(menuItem2.Id); //Should return menuItem2 because it was not deleted

            //Assert
            Assert.Null(newMenuItem);
            Assert.NotNull(newMenuItem2);
        }


        [Fact]
        public async Task DeleteMenuItem_ThatDoesNotExist_ShouldThrowItemDoesNotExistExeption()
        {
            //Arrange
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

            //Act
            var function = async () => await _repository.DeleteMenuItem(menuItem.Id);

            //Assert
            await Assert.ThrowsAsync<ItemDoesNotExistException>(function);
        }

        [Fact]
        public async Task UpdateMenuItem_ShouldUpdatePopulatedTable()
        {
            //Arrange
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

            var oldMenuItemCopy = new MenuItem
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                IconUrl = menuItem.IconUrl,
                BannerUrl = menuItem.BannerUrl,
                LongDescription = menuItem.LongDescription,
                ShortDescription = menuItem.ShortDescription,
                Price = menuItem.Price,
                CategoryId = menuItem.CategoryId
            };

            await _repository.InsertMenuItem(menuItem);

            //Act
            menuItem.Name = "boterham met kaas";
            menuItem.IconUrl = "https://kanikeenkortebroekaan.nl/";
            menuItem.BannerUrl = "https://kanikeenkortebroekaan.nl/";
            menuItem.LongDescription = "extra lang";
            menuItem.ShortDescription = "extra kort";
            menuItem.Price = 12.34;
            menuItem.CategoryId = 2;

            await _repository.UpdateMenuItem(menuItem);
            var updatedMenuItem = await _repository.GetMenuItemByID(menuItem.Id);


            //Asserts
            updatedMenuItem.Should()
                .NotBeNull()
                .And.NotBeEquivalentTo(oldMenuItemCopy)
                .And.BeEquivalentTo(menuItem);
        }

        [Fact]
        public async Task UpdateMenuItem_ThatDoesNotExist_ShouldThrowItemDoesNotExistExeption()
        {
            //Arrange
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

            //Act
            var function = async () => await _repository.UpdateMenuItem(menuItem);

            //Assert
            await Assert.ThrowsAsync<ItemDoesNotExistException>(function);
        }

    }
}
