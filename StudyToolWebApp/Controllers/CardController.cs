using Microsoft.AspNetCore.Mvc;
using StudyToolWebApp.Dto;
using StudyToolWebApp.Models;
using StudyToolWebApp.Repository.ClassRepository;
using StudyToolWebApp.Repository.InterfaceRepository;

namespace StudyToolWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : Controller
    {
        private readonly ICardRepository _cardRepository;
        private readonly IDeckRepository _deckRepository;

        public CardController(ICardRepository cardRepository, IDeckRepository deckRepository)
        {
            _cardRepository = cardRepository;
            _deckRepository = deckRepository;
        }

        [HttpGet]
        public IActionResult GetCards()
        {
            var cards = _cardRepository.GetCards();

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(cards);
        }

        [HttpGet("{id}")] 
        public IActionResult GetCard(int id) 
        {
            if(!_cardRepository.CardExists(id))
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

            if(!ModelState.IsValid)
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
                .Where(c => c.Term.Trim().ToLower() == cardDto.Term.Trim().ToLower())
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
    } 
}
