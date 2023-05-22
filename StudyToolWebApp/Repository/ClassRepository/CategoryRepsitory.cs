using StudyToolWebApp.Data;
using StudyToolWebApp.Models;
using StudyToolWebApp.Repository.InterfaceRepository;

namespace StudyToolWebApp.Repository.ClassRepository
{
    public class CategoryRepsitory : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepsitory(ApplicationDbContext context) 
        {
            _context = context;
        }
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.OrderBy(c => c.Id).ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Card> GetCardsByCategory(int CategoryId)
        {
            return _context.CardCategories.Where(c => c.CategoryId == CategoryId).Select(c => c.Card).ToList();
        }
    }
}
