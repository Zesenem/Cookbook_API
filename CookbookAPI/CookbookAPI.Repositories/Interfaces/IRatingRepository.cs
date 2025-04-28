using CookbookAPI.Domain;

namespace CookbookAPI.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task<List<Rating>> GetAllAsync();
        Task<Rating?> GetByIdAsync(int id);
        Task<Rating> AddAsync(Rating rating);
        Task<Rating> UpdateAsync(Rating rating);
        Task RemoveAsync(int id);
        Task<bool> GetAnyAsync(int id);
    }
}
