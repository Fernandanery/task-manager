using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;



        public UserService(IUserRepository repository, IMapper mapper, IPasswordHasher<User> passwordHasher)
        {
            _repository = repository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;

        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            user.Password = _passwordHasher.HashPassword(user, user.Password);
            return await _repository.AddAsync(user);
        }

        public async Task<User?> UpdateUserAsync(int id, UpdateUserDto updatedUser)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return null;

            _mapper.Map(updatedUser, user);

            await _repository.UpdateAsync(user);
            return user;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
