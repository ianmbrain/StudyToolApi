using StudyToolWebApp.Models;

namespace StudyToolWebApp.Repository.InterfaceRepository
{
    public interface IDeckRepository
    {
        ICollection<Deck> GetDecks();
        Deck GetDeck(int id);
        Deck GetDeckByCard(int cardId);
        ICollection<Card> GetCardsByDeck(int deckId);
        bool DecksExists(int id);
    }
}
