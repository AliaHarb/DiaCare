using AutoMapper;
using DiaCare.Application.Helpers;
using DiaCare.Application.Services;
using DiaCare.Domain.DTOS;
using DiaCare.Domain.Entities;
using DiaCare.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaCare.UnitTests
{
    [TestClass]
    public class LoginTests
    {
            private Mock<UserManager<ApplicationUser>> _mockUserManager;
            private Mock<IUnitOfWork> _mockUow;
            private Mock<IMapper> _mockMapper;
            private Mock<IOptions<JwtSettings>> _mockJwtOptions;
            private AuthService _authService;

            [TestInitialize]
            public void Setup()
            {
                // 1. Setup the Mock for UserManager (it requires a UserStore in its constructor)
                var store = new Mock<IUserStore<ApplicationUser>>();
                _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                    store.Object, null, null, null, null, null, null, null, null);

                _mockUow = new Mock<IUnitOfWork>();
                _mockMapper = new Mock<IMapper>();
                _mockJwtOptions = new Mock<IOptions<JwtSettings>>();

                // 2. Setup Mock for JWT Settings to ensure token generation logic doesn't crash
                _mockJwtOptions.Setup(o => o.Value).Returns(new JwtSettings
                {
                    Key = "A_Very_Strong_Secret_Key_For_Testing_123456789",
                    Issuer = "DiaCare",
                    Audience = "DiaCareUsers",
                    DurationInMinutes = 60
                });

                // 3. Create the AuthService instance with all dependencies mocked
                _authService = new AuthService(
                    _mockUserManager.Object,
                    _mockUow.Object,
                    _mockMapper.Object,
                    _mockJwtOptions.Object);
            }

            [TestMethod]
            public async Task LoginAsync_WithIncorrectPassword_ShouldReturnFailureMessage()
            {
                // Arrange: Setup data for a user with an incorrect password
                var loginDto = new LoginDto { Email = "test@user.com", Password = "WrongPassword" };
                var fakeUser = new ApplicationUser { Email = "test@user.com", UserName = "TestUser" };

                // Setup: Find the user successfully but return 'false' for the password check
                _mockUserManager.Setup(m => m.FindByEmailAsync(loginDto.Email))
                                .ReturnsAsync(fakeUser);

                _mockUserManager.Setup(m => m.CheckPasswordAsync(fakeUser, loginDto.Password))
                                .ReturnsAsync(false);

                // Act: Execute the Login method
                var result = await _authService.LoginAsync(loginDto);

                // Assert: Verify authentication failed and the correct error message was returned
                Assert.IsFalse(result.IsAuthenticated);
                Assert.AreEqual("Incorrect Email or Password.", result.Message);
                Assert.IsNull(result.Token);
            }
        }
    }
