using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class KitchenEntry
    {
        [Key]
        public int Id { get; set; }

        [Range(0, 3, MinimumIsExclusive = false, MaximumIsExclusive = false)]
        public int WeekNumber { get; set; }

        public int? UserId { get; set; }

        public AppUser? User { get; set; }

        public MonthModel Month { get; set; } = default!;

        public int MonthId { get; set; }
    }
}
