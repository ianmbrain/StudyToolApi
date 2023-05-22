using StudyToolWebApp.Data.Enums;
using StudyToolWebApp.Models;

namespace StudyToolWebApp.Dto
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CategoryColor Color { get; set; }
    }
}
