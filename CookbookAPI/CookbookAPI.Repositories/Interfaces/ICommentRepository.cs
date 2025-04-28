using CookbookAPI.Domain;

namespace CookbookAPI.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> AddAsync(Comment comment);
        Task<Comment> UpdateAsync(Comment comment);
        Task RemoveAsync(int id);
        Task<bool> GetAnyAsync(int id);
    }
}
