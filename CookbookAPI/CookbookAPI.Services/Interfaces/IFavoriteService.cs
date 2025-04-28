using CookbookAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookbookAPI.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task<List<Favorite>> GetAllAsync();
        Task<Favorite> GetByIdAsync(int id);
        Task<Favorite> CreateAsync(Favorite favorite);
        Task<bool> RemoveAsync(int userId, int recipeId);
    }
}
