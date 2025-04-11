using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services
{
    public class UserService(IUserRepository repository, IMapper mapper, IPasswordHasher<User> passwordHasher)
    {
        private readonly IUserRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

        public async Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync(cancellationToken);
        }

        public async Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _repository.GetUserByIdAsync(id, cancellationToken);
        }

        public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken)
        {
            user.Password = _passwordHasher.HashPassword(user, user.Password);
            return await _repository.AddUserAsync(user, cancellationToken);
        }

        public async Task<User?> UpdateUserAsync(int id, UpdateUserDto updatedUser, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserByIdAsync(id, cancellationToken);
            if (user == null) return null;

            _mapper.Map(updatedUser, user);

            await _repository.AddUserAsync(user, cancellationToken);
            return user;
        }

        public async Task<bool> DeleteUserAsync(int id, CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync(id, cancellationToken);
        }
    }
}
