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
        public CardController(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
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
        public IActionResult CreateCard([FromBody] CardDto cardDto)
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
                Deck = cardDto.DeckId
            };

            if (!_cardRepository.CreateCard(card))
            {
                ModelState.AddModelError("", "Unable to create card.");
            }

            return Ok("Created");
        }
    } 
}
