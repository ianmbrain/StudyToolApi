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

        [HttpPost]
        public IActionResult CreateDeck([FromBody] DeckDto deckDto)
        {
            if (deckDto == null)
            {
                return BadRequest(ModelState);
            }

            var deckExists = _deckRepository.GetDecks()
                .Where(c => c.Title.ToLower() == deckDto.Title.ToLower())
                .FirstOrDefault();

            if (deckExists != null)
            {
                ModelState.AddModelError("", "Deck already exists.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deck = new Deck
            {
                Title = deckDto.Title,
                Description = deckDto.Description

            };

            if (!_deckRepository.CreateDeck(deck))
            {
                ModelState.AddModelError("", "Unable to create deck.");
            }

            return Ok("Created");
        }

        [HttpPut("{deckId}")]
        public IActionResult UpdateDeck(int deckId, [FromBody] DeckDto deckDto)
        {
            if (deckDto == null)
            {
                return BadRequest(ModelState);
            }

            // Ensures that only the updated date field is updated, not the created date
            // No tracking is required as entity framework can't track two cards at once
            DateTime createdDate = _deckRepository.GetDeckNoTracking(deckId).CreatedAt;

            if (deckId != deckDto.Id)
            {
                return BadRequest(ModelState);
            }

            if(!_deckRepository.DecksExists(deckId))
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deck = new Deck
            {
                Id = deckDto.Id,
                Title = deckDto.Title,
                Description = deckDto.Description,
                CreatedAt = createdDate
            };

            if(!_deckRepository.UpdateDeck(deck))
            {
                ModelState.AddModelError("", "Unable to update deck");
            }

            return NoContent();
        }

        [HttpDelete("{deckId}")]
        public IActionResult DeleteDeck(int deckId)
        {
            if(!_deckRepository.DecksExists(deckId))
            {
                return NotFound();
            }

            var deck = _deckRepository.GetDeck(deckId);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!_deckRepository.DeleteDeck(deck))
            {
                ModelState.AddModelError("", "Unable to delete deck");
            }

            return NoContent();
        }
    }
}
