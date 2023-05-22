namespace StudyToolWebApp.Models
{
    public class Deck
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Card> Cards { get; set; }
    }
}
