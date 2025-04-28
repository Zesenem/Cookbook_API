using CookbookAPI.Domain;
using CookbookAPI.Repositories.Interfaces;
using CookbookAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CookbookAPI.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> GetUserProfileAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task AddFavoriteRecipeAsync(int userId, int recipeId)
        {
            await _userRepository.AddFavoriteRecipeAsync(userId, recipeId);
        }

        public async Task<List<Recipe>> GetUserFavoriteRecipesAsync(int userId)
        {
            return await _userRepository.GetFavoriteRecipesAsync(userId);
        }

        public async Task BlockUserAsync(int id)
        {
            await _userRepository.BlockUserAsync(id);
        }

        public async Task UnblockUserAsync(int id)
        {
            await _userRepository.UnblockUserAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User> RegisterAsync(User user)
        {
            if (await _userRepository.UserExistsByEmailAsync(user.Email))
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }
            user.Password = HashPassword(user.Password);
            user.Role = Role.RegisteredUser;
            return await _userRepository.AddAsync(user);
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !VerifyPassword(password, user.Password)) 
            {
                return null; 
            }
            return user;
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }

        public async Task<User> UpdateAsync(User user)
        {
            if (!string.IsNullOrEmpty(user.Password))
            {
                user.Password = HashPassword(user.Password);
            }
            return await _userRepository.UpdateAsync(user);
        }

        public async Task RemoveAsync(int id)
        {
            await _userRepository.RemoveAsync(id);
        }
    }
}
