using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BreakfastsController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateBreakfast(CreateBreakfastRequest request)
        {
            var breakfest = new Breakfast(
                Guid.NewGuid(),
                request.Name,
                request.Description,
                request.StartDateTime,
                request.EndDateTime,
                DateTime.UtcNow,
                request.Savory,
                request.Sweet);

            var response = new CreateBreakfastResponse(
                breakfest.Id,
                breakfest.Name,
                breakfest.Description,
                breakfest.StartDateTime,
                breakfest.EndDateTime,
                breakfest.LastModifiedDateTime,
                breakfest.Savory,
                breakfest.Sweet);

            return CreatedAtAction(
                actionName: nameof(GetBreakfast),
                routeValues: new {id =  breakfest.Id},
                value: request);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetBreakfast(Guid id)
        {
            return Ok(id);
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
        {
            return Ok(request);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteBreakfast(Guid id)
        {
            return Ok(id);
        }
    }
}
