using Layer_Entities.ModelsDTO;
using Layer_Entities.Wrappers;
using Layer_Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace UserManagementAPI.Test
{
    public class AccountControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly AccountController _accountController;

        public AccountControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _accountController = new AccountController(_mockUserService.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_NullRequest_ReturnsNoContent()
        {
            // Act
            var result = await _accountController.AuthenticateAsync(null);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AuthenticateAsync_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new AuthenticationRequest
            {
                Email = "test@example.com",
                Password = "password123" 
            };

            var response = new JsonResponse<AuthenticationResponse>
            {
                Data = new AuthenticationResponse
                {
                    Id = "user-id-123",
                    Created = DateTime.UtcNow,
                    Modified = DateTime.UtcNow,
                    LastLogin = DateTime.UtcNow,
                    IsActive = true,
                    HasError = false,
                    JWToken = "sample-jwt-token", 
                    RefreshToken = "sample-refresh-token" 
                },
                Errors = new List<string>() 
            };


            _mockUserService.Setup(service => service.LoginAsync(request)).ReturnsAsync(response);

            // Act
            var result = await _accountController.AuthenticateAsync(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResponse = Assert.IsType<JsonResponse<AuthenticationResponse>>(okResult.Value);
            Assert.Equal(response.Data.Id, actualResponse.Data.Id); 
            Assert.Equal(response.Data.JWToken, actualResponse.Data.JWToken); 
            Assert.Equal(response.Data.HasError, actualResponse.Data.HasError); 
        }

        [Fact]
        public async Task RegisterUserAsync_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new RegisterRequest
            {
                NameUser = "Juan Perez",
                Email = "juan.perez@bhd.com",
                PasswordHash = "ContraseñaSegura123",
                IsActive = true,
                Phones = new List<PhonesDTO>
                {
                    new PhonesDTO
                    {
                        Number = "1234567890",
                        City_Code = "809",
                        Country_Code = "1"
                    }
                }
            };

            var registerResponse = new JsonResponse<RegisterResponseUser>
            {
                Data = new RegisterResponseUser
                {
                    Id = "user-id-123",
                    Created = DateTime.UtcNow,
                    Modified = DateTime.UtcNow,
                    LastLogin = DateTime.UtcNow,
                    IsActive = true,
                    HasError = false,
                    JWToken = "sample-jwt-token",
                    RefreshToken = "sample-refresh-token"
                },
                Errors = new List<string>()
            };

            _mockUserService.Setup(service => service.RegisterUsers(request)).ReturnsAsync(registerResponse);

            // Act
            var result = await _accountController.RegisterUserAsync(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResponse = Assert.IsType<JsonResponse<RegisterResponseUser>>(okResult.Value);
            Assert.NotNull(actualResponse.Data);
            Assert.Empty(actualResponse.Errors); 
        }

        [Fact]
        public async Task RegisterUserAsync_InvalidRequest_ReturnsBadRequest()
        {
                var request = new RegisterRequest
                {
                    NameUser = "Yahinniel Perez",
                    Email = "Yahinniel.pez@bhd.com",
                    PasswordHash = "ContraseñaSegura123",
                    IsActive = true,
                    Phones = new List<PhonesDTO>
                    {
                        new PhonesDTO
                        {
                            Number = "1234567890",  
                            City_Code = "809",       
                            Country_Code = "1"       
                        }
                    }
                };

            var registerResponse = new JsonResponse<RegisterResponseUser>
            {
                Errors = new List<string> { "Email ya registrado." }
            };

            _mockUserService.Setup(service => service.RegisterUsers(request)).ReturnsAsync(registerResponse);

            var result = await _accountController.RegisterUserAsync(request);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsAssignableFrom<IEnumerable<string>>(badRequestResult.Value);
            Assert.NotEmpty(errors);
        }
    }
}
