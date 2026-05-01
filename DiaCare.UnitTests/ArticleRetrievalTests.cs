using AutoMapper;
using DiaCare.Application.DTOS;
using DiaCare.Application.Services;
using DiaCare.Domain.Entities;
using DiaCare.Domain.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;

namespace DiaCare.UnitTests
{
    [TestClass]
    public class ArticleRetrievalTests
    {
        private Mock<IBaseRepository<Article>> _mockRepo;
        private Mock<IUnitOfWork> _mockUow;
        private Mock<IMapper> _mockMapper;
        private ArticleService _service;
        [TestInitialize]
        public void Setup()
        {
            // intializing the mocks and the service before each test
            _mockRepo = new Mock<IBaseRepository<Article>>();
            _mockUow = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _service = new ArticleService(_mockRepo.Object, _mockUow.Object, _mockMapper.Object);
        }

        [TestMethod]
        public async Task GetArticleById_ShouldReturnCorrectData()
        {
            // 1. Arrange: (Setup Mocks)
            int expectedId = 1;
            string expectedTitle = "Diabetes Care 101";

            // returning a fake article when the repository's GetByIdAsync method is called with the expected ID
            var fakeArticle = new Article { Id = 1, Title = "Diabetes Care 101" };
            var fakeDto = new ArticleDto { Id = 1, Title = "Diabetes Care 101" };

            _mockRepo.Setup(repo => repo.GetByIdAsync(expectedId, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(fakeArticle);
            _mockMapper.Setup(m => m.Map<ArticleDto>(fakeArticle)).Returns(fakeDto);


            // 2. Act
            var result = await _service.GetArticleByIdAsync(expectedId);

            // 3. Assert 
            Assert.IsNotNull(result); // Is Data Returned? (Not Null)
            Assert.AreEqual(expectedId, result.Id); // Is ID is correct?
            Assert.AreEqual(expectedTitle, result.Title); // Is Title is correct?
        }
    }
}