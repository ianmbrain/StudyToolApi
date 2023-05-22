using StudyToolWebApp.Models;

namespace StudyToolWebApp.Repository.InterfaceRepository
{
    public interface ICardRepository
    {
        ICollection<Card> GetCards();
    }
}
