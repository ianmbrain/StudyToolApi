using StudyToolWebApp.Data.Enums;

namespace StudyToolWebApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CategoryColor Color { get; set; }
        public ICollection<CardCategory> Cards { get; set; }
    }
}
