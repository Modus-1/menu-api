using FluentAssertions;
using menu_api.Controllers;
using menu_api.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions.Execution;
using Xunit;
using menu_api.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace menu_api.Tests.ControllerTests
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryRepository> _repository;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _repository = new Mock<ICategoryRepository>();
            _controller = new CategoryController(_repository.Object);
        }

        [Fact]
        public async Task GetAllCategories_WithSeededDatabase_ShouldReturnOk()
        {
            // Arrange
            IEnumerable<Category> data = new List<Category>()
            {
                new (),
                new (),
                new (),
                new (),
                new ()
            };
            _repository.Setup(repository => repository.GetAllCategories()).ReturnsAsync(data);

            // Act
            var result = await _controller.GetAllCategories();
            var okResult = result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                okResult.Should().NotBeNull();
                okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
                okResult?.Value.Should().BeOfType<List<Category>>();
            }
        }
        
        [Fact]
        public async Task GetAllCategories_WithEmptyDatabase_ShouldReturnOk()
        {
            // Arrange
            IEnumerable<Category> data = new List<Category>();
            _repository.Setup(repository => repository.GetAllCategories()).ReturnsAsync(data);

            // Act
            var result = await _controller.GetAllCategories();
            var okResult = result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                okResult.Should().NotBeNull();
                okResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
                okResult?.Value.Should().BeOfType<List<Category>>();
            }
        }
    }
}