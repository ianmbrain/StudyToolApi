using StudyToolWebApp.Data;
using StudyToolWebApp.Models;
using StudyToolWebApp.Repository.InterfaceRepository;

namespace StudyToolWebApp.Repository.ClassRepository
{
    public class CardRepository : ICardRepository
    {
        private readonly ApplicationDbContext _context;
        public CardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CardExists(int id)
        {
            return _context.cards.Any(c => c.Id == id);
            
        }

        public Card GetCard(int id)
        {
            return _context.cards.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Card> GetCards()
        {
            return _context.cards.OrderBy(c => c.Id).ToList();
        }
    }
}
