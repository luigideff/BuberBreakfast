using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using BuberBreakfast.ServiceErrors;
using BuberBreakfast.Services.Breakfasts;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers
{
    
    public class BreakfastsController : ApiController
    {
        private readonly IBreakfastService _breakfastService;

        public BreakfastsController(IBreakfastService breakfastService)
        {
            _breakfastService = breakfastService;
        }

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

            ErrorOr<Created> createBreakfastResult = _breakfastService.CreateBreakfast(breakfest);

            return createBreakfastResult.Match(
                created => CreatedAsGetBreakfast(breakfest),
                errors => Problem(errors));
        }        

        [HttpGet("{id:guid}")]
        public IActionResult GetBreakfast(Guid id)
        {
            ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);

            return getBreakfastResult.Match(
                breakfast => Ok(MapBreakfastResponse(breakfast)),
                errors => Problem(errors));
        }        

        [HttpPut("{id:guid}")]
        public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
        {
            var breakfest = new Breakfast(
                id,
                request.Name,
                request.Description,
                request.StartDateTime,
                request.EndDateTime,
                DateTime.UtcNow,
                request.Savory,
                request.Sweet);

            ErrorOr<UpsertedBreakfast> upsertBreakfastResult = _breakfastService.UpsertBreakfast(breakfest);

            return upsertBreakfastResult.Match(
                upserted => upserted.IsNewlyCreated ? CreatedAsGetBreakfast(breakfest) : NoContent(),
                errors => Problem(errors));
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteBreakfast(Guid id)
        {
            ErrorOr<Deleted> deleteBreakfastResult =  _breakfastService.DeleteBreakfast(id);

            return deleteBreakfastResult.Match(
                deleted => NoContent(),
                errors => Problem(errors));
        }

        private static CreateBreakfastResponse MapBreakfastResponse(Breakfast breakfast)
        {
            return new CreateBreakfastResponse(
                            breakfast.Id,
                            breakfast.Name,
                            breakfast.Description,
                            breakfast.StartDateTime,
                            breakfast.EndDateTime,
                            breakfast.LastModifiedDateTime,
                            breakfast.Savory,
                            breakfast.Sweet);
        }

        private IActionResult CreatedAsGetBreakfast(Breakfast breakfest)
        {
            return CreatedAtAction(
                actionName: nameof(GetBreakfast),
                routeValues: new { id = breakfest.Id },
                value: MapBreakfastResponse(breakfest));
        }
    }
}
