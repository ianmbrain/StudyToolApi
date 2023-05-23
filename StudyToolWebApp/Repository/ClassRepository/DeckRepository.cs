using StudyToolWebApp.Data;
using StudyToolWebApp.Models;
using StudyToolWebApp.Repository.InterfaceRepository;

namespace StudyToolWebApp.Repository.ClassRepository
{
    public class DeckRepository : IDeckRepository
    {
        private readonly ApplicationDbContext _context;

        public DeckRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool DecksExists(int id)
        {
            return _context.Decks.Any(d => d.Id == id);
        }

        public ICollection<Card> GetCardsByDeck(int deckId)
        {
            return _context.cards.Where(c =>  c.Deck.Id == deckId).ToList();
        }

        public Deck GetDeck(int id)
        {
            return _context.Decks.Where(c => c.Id == id).FirstOrDefault();
        }

        public Deck GetDeckByCard(int cardId)
        {
            return _context.cards.Where(c => c.Id == cardId).Select(d => d.Deck).FirstOrDefault();
        }

        public ICollection<Deck> GetDecks()
        {
            return _context.Decks.OrderBy(d => d.Id).ToList();
        }
    }
}
