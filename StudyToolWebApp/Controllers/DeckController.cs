using Microsoft.AspNetCore.Mvc;
using StudyToolWebApp.Dto;
using StudyToolWebApp.Models;
using StudyToolWebApp.Repository.ClassRepository;
using StudyToolWebApp.Repository.InterfaceRepository;

namespace StudyToolWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeckController : Controller
    {
        private readonly IDeckRepository _deckRepository;
        private readonly ICardRepository _cardRepository;

        public DeckController(IDeckRepository deckRepository, ICardRepository cardRepository)
        {
            _deckRepository = deckRepository;
            _cardRepository = cardRepository;
        }

        [HttpGet]
        public IActionResult GetDecks()
        {
            var decks = _deckRepository.GetDecks();

            ICollection<DeckDto> deckDtos = new List<DeckDto>();

            foreach (Deck d in decks)
            {
                deckDtos.Add(new DeckDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Description = d.Description
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(deckDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GedDeck(int id)
        {
            if (!_deckRepository.DecksExists(id))
            {
                return NotFound();
            }

            var deck = _deckRepository.GetDeck(id);
            var deckDto = new DeckDto
            {
                Id = deck.Id,
                Title = deck.Title,
                Description = deck.Description
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(deckDto);
        }

        [HttpGet("/cards/{cardId}")]
        public IActionResult GetDeckByCard(int cardId)
        {
            if (!_cardRepository.CardExists(cardId))
            {
                return NotFound();
            }

            var deck = _deckRepository.GetDeckByCard(cardId);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DeckDto deckDto = new DeckDto
            {
                Id = deck.Id,
                Title = deck.Title,
                Description = deck.Description
            };

            return Ok(deckDto);
        }

        [HttpGet("/decks/{deckId}")]
        public IActionResult GetCardsByDeck(int deckId)
        {
            if(!_deckRepository.DecksExists(deckId))
            {
                return NotFound();
            }

            var cards = _deckRepository.GetCardsByDeck(deckId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<CardDto> cardDtos = new List<CardDto>();

            foreach (Card c in cards) {
                cardDtos.Add(new CardDto
                {
                    Id = c.Id,
                    Term = c.Term,
                    Description = c.Description,
                    Important = c.Important
                });
            };

            return Ok(cardDtos);
        }
    }
}
