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
	public class ArticleDeleteTests
	{
		private Mock<IBaseRepository<Article>> mockRepo;
		private Mock<IUnitOfWork> mockUow;
		private Mock<IMapper> mockMapper;
		private ArticleService service;

		[TestInitialize]
		public void Setup()
		{
			mockRepo = new Mock<IBaseRepository<Article>>();
			mockUow = new Mock<IUnitOfWork>();
			mockMapper = new Mock<IMapper>();
			service = new ArticleService(mockRepo.Object, mockUow.Object, mockMapper.Object);
		}

		[TestMethod]
		public async Task DeleteArticleAsync_WhenArticleExists_ShouldReturnTrue()
		{
			// Arrange
			int id = 1;
			var fakeArticle = new Article { Id = id, Title = "Diabetes Care 101" };

			mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
					.ReturnsAsync(fakeArticle);

			// Act
			var result = await service.DeleteArticleAsync(id);

			// Assert
			Assert.IsTrue(result);
			mockRepo.Verify(r => r.DeleteAsync(fakeArticle, It.IsAny<CancellationToken>()), Times.Once);
			mockUow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
		}

		[TestMethod]
		public async Task DeleteArticleAsync_WhenArticleNotFound_ShouldReturnFalse()
		{
			// Arrange
			int id = 99;

			mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
					.ReturnsAsync((Article)null);

			// Act
			var result = await service.DeleteArticleAsync(id);

			// Assert
			Assert.IsFalse(result);
			mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Article>(), It.IsAny<CancellationToken>()), Times.Never);
			mockUow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
		}
	}
}

