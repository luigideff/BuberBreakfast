using BuberBreakfast.Models;
using BuberBreakfast.ServiceErrors;
using ErrorOr;

namespace BuberBreakfast.Services.Breakfasts
{
    public class BreakfastService : IBreakfastService
    {
        private static readonly Dictionary<Guid, Breakfast> _breakfasts = new Dictionary<Guid, Breakfast>();
        public ErrorOr<Created> CreateBreakfast(Breakfast breakfast) 
        {
            _breakfasts.Add(breakfast.Id, breakfast);
            return Result.Created;
        }

        public ErrorOr<Deleted> DeleteBreakfast(Guid id)
        {
            _breakfasts.Remove(id);
            return Result.Deleted;
        }

        public ErrorOr<Breakfast> GetBreakfast(Guid id)
        {
            if (_breakfasts.TryGetValue(id, out var breakfast))
            {
                return breakfast;
            }
            return Errors.Breakfast.NotFound;
        }

        public ErrorOr<UpsertedBreakfast> UpsertBreakfast(Breakfast breakfest)
        {
            var isNewlyCreated = !_breakfasts.ContainsKey(breakfest.Id);
            _breakfasts[breakfest.Id] = breakfest;

            return new UpsertedBreakfast(isNewlyCreated);
        }
    }
}
