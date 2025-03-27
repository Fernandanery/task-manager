using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
        Task<User?> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
    }
}