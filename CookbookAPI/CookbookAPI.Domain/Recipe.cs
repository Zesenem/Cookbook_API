namespace CookbookAPI.Domain
{
    public class Recipe
    {
     
        public int Id { get; set; }
        public string Name { get; set; }
        public string Instructions { get; set; }
        public string Ingredients { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string Difficulty { get; set; }
        public int Duration { get; set; }

        public List<Comment> Comments { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public List<Favorite> Favorites { get; set; }

        public bool IsApproved { get; set; } = false;
        public DateTime? SubmissionDate { get; set; }
    }
}
