using Server.Data;
using Server.Models;

namespace Server.Services
{
    public class InitNextMonthService(AppDbContext ctx) : IInitNextMonthService
    {
        public async Task CreateNextMonthAsync()
        {
            DateTime dateTomorrow = DateTime.UtcNow;
            int monthNumber = dateTomorrow.Month;
            int yearNumber = dateTomorrow.Year;

            MonthModel mm = new()
            {
                Month = monthNumber,
                Year = yearNumber,
                BathroomEntries = CreateBathroomEntries(),
                KitchenEntries = CreateKitchenEntries(),
                ShoppingEntries = []
            };

            await ctx.Months.AddAsync(mm).ConfigureAwait(false);

            await ctx.SaveChangesAsync().ConfigureAwait(false);
        }

        private static IList<BathroomEntry> CreateBathroomEntries() => Enumerable.Range(0, 3)
            .Select(i => new BathroomEntry() { WeekNumber = i }).ToList();

        private static IList<KitchenEntry> CreateKitchenEntries() => Enumerable.Range(0, 3)
            .Select(i => new KitchenEntry() { WeekNumber = i }).ToList();
    }
}