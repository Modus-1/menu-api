using FluentAssertions;
using menu_api.Context;
using menu_api.Models;
using menu_api.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;
using menu_api.Exeptions;
using menu_api.Repositories.Interfaces;

namespace menu_api.Tests.RepositoryTests
{
    public class CategoryRepositoryTests : IDisposable
    {
        private readonly CategoryRepository _repository;
        private readonly MenuContext _context;

        public CategoryRepositoryTests()
        {
            var options =
                new DbContextOptionsBuilder<MenuContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;
            _context = new MenuContext(options);
            _repository = new CategoryRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task GetAllCategories_WithSeededDatabase_ShouldReturnAllCategories()
        {
            // Arrange
            for (var i = 0; i < 5; i++)
            {
                await _context.Categories.AddAsync(new Category());
            }
            await _context.SaveChangesAsync();
            const int expectedCount = 5;
            
            // Act
            var results = await _repository.GetAllCategories();

            // Assert
            results.Should().NotBeNull().And.NotBeEmpty().And.HaveCount(expectedCount);
        }
        
        [Fact]
        public async Task GetAllCategories_WithEmptyDatabase_ShouldReturnEmptyList()
        {
            // Arrange
            
            // Act
            var results = await _repository.GetAllCategories();

            // Assert
            results.Should().NotBeNull().And.BeEmpty();
        }
    }
}