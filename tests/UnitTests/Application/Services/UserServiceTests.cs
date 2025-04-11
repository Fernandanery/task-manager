using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.UnitTests.Application.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _passwordHasherMock.Object
            );
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsUsers()
        {
            var users = new List<User> { new User { Id = 1, Name = "Fernanda", Email = "fernanda@test.com", Password = "123" } };
            _userRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);

            var result = await _userService.GetAllUsersAsync(CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("Fernanda", result[0].Name);
        }

        [Fact]
        public async Task GetUserByIdAsync_ExistingId_ReturnsUser()
        {
            var user = new User { Id = 1, Name = "Fernanda", Email = "fernanda@test.com", Password = "123" };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var result = await _userService.GetUserByIdAsync(1, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("Fernanda", result.Name);
        }

        [Fact]
        public async Task CreateUserAsync_HashesPasswordAndCreatesUser()
        {
            var user = new User { Name = "Fernanda", Email = "fernanda@test.com", Password = "123" };
            _passwordHasherMock.Setup(ph => ph.HashPassword(user, user.Password)).Returns("hashed-password");
            _userRepositoryMock.Setup(repo => repo.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);

            var result = await _userService.CreateUserAsync(user, CancellationToken.None);

            Assert.Equal("hashed-password", result.Password);
        }

        [Fact]
        public async Task UpdateUserAsync_ValidId_UpdatesUser()
        {
            var existingUser = new User { Id = 1, Name = "Fernanda", Email = "fernanda@test.com", Password = "123" };
            var updatedDto = new UpdateUserDto { Name = "Nery", Email = "nery@test.com" };

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(repo => repo.AddUserAsync(existingUser, It.IsAny<CancellationToken>())).ReturnsAsync(existingUser);

            _mapperMock.Setup(m => m.Map(updatedDto, existingUser));

            var result = await _userService.UpdateUserAsync(1, updatedDto, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteUserAsync_ReturnsTrue()
        {
            _userRepositoryMock.Setup(repo => repo.DeleteAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await _userService.DeleteUserAsync(1, CancellationToken.None);

            Assert.True(result);
        }
    }
}