using AutoMapper;
using DiaCare.Application.DTOS;
using DiaCare.Application.Services;
using DiaCare.Domain.Entities;
using DiaCare.Domain.Interfaces;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaCare.UnitTests
{
    [TestClass]
    public class ArticleAddTests
    {
        private readonly Mock<IBaseRepository<Article>> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ArticleService _service;

        public ArticleAddTests()
        {
            _mockRepo = new Mock<IBaseRepository<Article>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();

            _service = new ArticleService(
                _mockRepo.Object,
                _mockUnitOfWork.Object,
                _mockMapper.Object
            );
        }

        // Test 1
        [TestMethod]
        public async Task AddArticleAsync_ValidDto_ReturnsCreatedArticle()
        {
            // Arrange
            var inputDto = new ArticleDto { Title = "New Article" };
            var article = new Article { Title = "New Article" };
            var returnedDto = new ArticleDto { Id = 1, Title = "New Article" };

            _mockMapper.Setup(m => m.Map<Article>(inputDto)).Returns(article);
            _mockRepo.Setup(r => r.AddAsync(article, default)).ReturnsAsync(article);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<ArticleDto>(article)).Returns(returnedDto);

            // Act
            var result = await _service.AddArticleAsync(inputDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("New Article", result.Title);
        }

        // Test 2 
        [TestMethod]
        public async Task AddArticleAsync_ValidDto_CallsSaveChangesOnce()
        {
            // Arrange
            var inputDto = new ArticleDto { Title = "Test" };
            var article = new Article { Title = "Test" };

            _mockMapper.Setup(m => m.Map<Article>(inputDto)).Returns(article);
            _mockRepo.Setup(r => r.AddAsync(article, default)).ReturnsAsync(article);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<ArticleDto>(article)).Returns(new ArticleDto());

            // Act
            await _service.AddArticleAsync(inputDto);

            // Assert
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }
    }
}
