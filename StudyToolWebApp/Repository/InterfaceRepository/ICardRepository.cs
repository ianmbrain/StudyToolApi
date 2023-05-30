using StudyToolWebApp.Models;

namespace StudyToolWebApp.Repository.InterfaceRepository
{
    public interface ICardRepository
    {
        ICollection<Card> GetCards();
        Card GetCard(int id);
        bool CardExists(int id);
        bool CardCategoryExists(int cardId, int categoryId);
        bool CreateCard(int categoryId, Card card);
        bool UpdateCard(Card card);
        bool DeleteCard(Card card);
        bool AddCardToCategory(CardCategory cardCategory);
        ICollection<Category> GetCategoriesByCard(int cardId);
        bool Save();
    }
}
