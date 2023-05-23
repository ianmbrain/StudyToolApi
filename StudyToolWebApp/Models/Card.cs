namespace StudyToolWebApp.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Term { get; set; }
        public string Description { get; set; }
        public bool Important { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
        public Deck Deck { get; set;}
        public ICollection<CardCategory> Categories { get; set; }
    }
}
