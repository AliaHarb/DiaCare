using AutoMapper;
using DiaCare.Application.DTOS;
using DiaCare.Application.Services;
using DiaCare.Domain.Entities;
using DiaCare.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaCare.UnitTests
{
    [TestClass]
    public class ArticleUpdateTests
    {
        private Mock<IBaseRepository<Article>> _mockRepo;
        private Mock<IUnitOfWork> _mockUow;
        private Mock<IMapper> _mockMapper;
        private ArticleService _service;
        [TestInitialize]
        public void Setup()
        {
            // Initialize Mock objects for the dependencies
            _mockRepo = new Mock<IBaseRepository<Article>>();
            _mockUow = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();

            // Inject the mocked objects into the ArticleService
            _service = new ArticleService(_mockRepo.Object, _mockUow.Object, _mockMapper.Object);
        }

        [TestMethod]
        public async Task UpdateArticleAsync_WhenValid_ShouldReturnUpdatedDto()
        {
            // Arrange: Define the expected data
            int id = 1;
            var existingArticle = new Article { Id = id, Title = "Old Title" };
            var inputDto = new ArticleDto { Title = "New Title" };
            var outputDto = new ArticleDto { Id = id, Title = "New Title" };

            // Setup: Tell the Mock Repository to return the existing article when searched by ID
            _mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(existingArticle);

            // Setup: Mock the AutoMapper to handle the conversion between DTOs and Entities
            _mockMapper.Setup(m => m.Map(inputDto, existingArticle));
            _mockMapper.Setup(m => m.Map<ArticleDto>(existingArticle)).Returns(outputDto);

            // Setup: Mock the Unit of Work to simulate successful database saving
            _mockUow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act: Execute the service method
            var result = await _service.UpdateArticleAsync(id, inputDto);

            // Assert: Verify the results are as expected
            Assert.IsNotNull(result);
            Assert.AreEqual("New Title", result.Title);

            // Verify: Ensure SaveChangesAsync was actually called once
            _mockUow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task UpdateArticleAsync_WhenArticleNotFound_ShouldReturnNull()
        {
            // Arrange: Use an ID that does not exist
            int nonExistentId = 99;
            _mockRepo.Setup(r => r.GetByIdAsync(nonExistentId, It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Article)null);

            // Act: Try to update a non-existent article
            var result = await _service.UpdateArticleAsync(nonExistentId, new ArticleDto());

            // Assert: The result should be null due to the Guard Clause
            Assert.IsNull(result);

            // Verify: Ensure the repository's Update and SaveChanges were NEVER called
            _mockRepo.Verify(r => r.Update(It.IsAny<Article>()), Times.Never);
            _mockUow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

    }
}
