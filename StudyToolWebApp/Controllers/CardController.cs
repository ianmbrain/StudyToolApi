using Microsoft.AspNetCore.Mvc;
using StudyToolWebApp.Dto;
using StudyToolWebApp.Models;
using StudyToolWebApp.Repository.ClassRepository;
using StudyToolWebApp.Repository.InterfaceRepository;
using System.Drawing;

namespace StudyToolWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : Controller
    {
        private readonly ICardRepository _cardRepository;
        private readonly IDeckRepository _deckRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CardController(ICardRepository cardRepository, IDeckRepository deckRepository, ICategoryRepository categoryRepository)
        {
            _cardRepository = cardRepository;
            _deckRepository = deckRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult GetCards()
        {
            var cards = _cardRepository.GetCards();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(cards);
        }

        [HttpGet("{id}")]
        public IActionResult GetCard(int id)
        {
            if (!_cardRepository.CardExists(id))
            {
                return NotFound();
            }

            var card = _cardRepository.GetCard(id);
            var cardDto = new CardDto
            {
                Id = card.Id,
                Term = card.Term,
                Description = card.Description,
                Important = card.Important
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(cardDto);
        }

        [HttpPost]
        public IActionResult CreateCard([FromQuery] int deckId, [FromQuery] int categoryId, [FromBody] CardDto cardDto)
        {
            if (cardDto == null)
            {
                return BadRequest(ModelState);
            }

            var cardExists = _cardRepository.GetCards()
                .Where(c => c.Term.ToLower() == cardDto.Term.ToLower())
                .FirstOrDefault();

            if (cardExists != null)
            {
                ModelState.AddModelError("", "Card already exists.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var card = new Card
            {
                Term = cardDto.Term,
                Description = cardDto.Description,
                Important = cardDto.Important,
                Deck = _deckRepository.GetDeck(deckId)
            };

            if (!_cardRepository.CreateCard(categoryId, card))
            {
                ModelState.AddModelError("", "Unable to create card.");
            }

            return Ok("Created");
        }

        [HttpPut("{cardId}")]
        public IActionResult UpdateCard(int cardId, [FromQuery] int deckId, [FromBody] CardDto cardDto)
        {
            if (cardDto == null)
            {
                return BadRequest(ModelState);
            }

            // Ensures that only the updated date field is updated, not the created date
            // No tracking is required as entity framework can't track two cards at once
            DateTime createdDate = _cardRepository.GetCardNoTracking(cardId).CreatedAt;

            if (cardId != cardDto.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_cardRepository.CardExists(cardId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var card = new Card
            {
                Id = cardDto.Id,
                Term = cardDto.Term,
                Description = cardDto.Description,
                Important = cardDto.Important,
                CreatedAt = createdDate,
                Deck = _deckRepository.GetDeck(deckId)
            };

            if (!_cardRepository.UpdateCard(card))
            {
                ModelState.AddModelError("", "Unable to update card");
            }

            return NoContent();
        }

        [HttpDelete("{cardId}")]
        public IActionResult DeleteCard(int cardId)
        {
            if (!_cardRepository.CardExists(cardId))
            {
                return NotFound();
            }

            var card = _cardRepository.GetCard(cardId);

            if (!_cardRepository.DeleteCard(card))
            {
                ModelState.AddModelError("", "Unable to delete card");
            }

            return NoContent();
        }

        [HttpPost("/CreateCard")]
        public IActionResult AddCategory([FromQuery] int cardId, [FromQuery] int categoryId)
        {
            if (_cardRepository.CardCategoryExists(cardId, categoryId))
            {
                ModelState.AddModelError("", "Card category already exists.");
            }

            if (!_cardRepository.AddCardToCategory(cardId, categoryId))
            {
                ModelState.AddModelError("", "Unable to add card category");
            }

            return Ok("Added");
        }

        [HttpDelete("DeleteCardCategory")]
        public IActionResult DeleteCardCategory([FromQuery] int cardId, [FromQuery] int categoryId)
        {
            if (!_cardRepository.CardCategoryExists(cardId, categoryId))
            {
                return NotFound();
            }

            var cardCategory = _cardRepository.GetCardCategory(cardId, categoryId);

            if (!_cardRepository.DeleteCardCategory(cardCategory))
            {
                ModelState.AddModelError("", "Unable to delete card category");
            }

            return NoContent();
        }


        [HttpGet("Categories/{cardId}")]
        public IActionResult GetCategoriesByCard(int cardId)
        {
            if (!_cardRepository.CardExists(cardId))
            {
                return NotFound();
            }

            var categories = _cardRepository.GetCategoriesByCard(cardId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<CategoryDto> categoryDtos = new List<CategoryDto>();

            foreach (Category c in categories)
            {
                categoryDtos.Add(new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Color = c.Color
                });
            };

            return Ok(categoryDtos);
        }
    }
}
