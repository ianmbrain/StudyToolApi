using StudyToolWebApp.Models;
using System.Diagnostics.Eventing.Reader;

namespace StudyToolWebApp.Repository.InterfaceRepository
{
    public interface IDeckRepository
    {
        ICollection<Deck> GetDecks();
        Deck GetDeck(int id);
        Deck GetDeckByCard(int cardId);
        ICollection<Card> GetCardsByDeck(int deckId);
        bool DecksExists(int id);
        bool CreateDeck(Deck deck);
        bool UpdateDeck(Deck deck);
        bool Save();
    }
}
