using Microsoft.AspNetCore.Mvc;
using StudyToolWebApp.Dto;
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
    } 
}
