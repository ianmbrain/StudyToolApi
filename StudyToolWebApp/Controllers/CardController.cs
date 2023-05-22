using Microsoft.AspNetCore.Mvc;
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
    }
}
