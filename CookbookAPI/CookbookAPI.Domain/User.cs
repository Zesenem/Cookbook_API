namespace CookbookAPI.Domain
{
    public class User
    {
        public User() 
        {
            FavoriteRecipes = new List<Recipe>();

            Comments = new List<Comment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; } = Role.Guest;
        public List<Recipe> FavoriteRecipes { get; set; }
        public List<Comment> Comments { get; set; }
        public bool IsBlocked { get; set; } = false;
    }

    public enum Role
    {
        Guest = 0,
        RegisteredUser = 1,
        Admin = 2
    }
}
