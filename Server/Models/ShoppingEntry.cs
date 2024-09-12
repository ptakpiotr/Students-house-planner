using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class ShoppingEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(255)]
        public string Item { get; set; } = default!;

        public AppUser Person { get; set; } = default!;

        public int PersonId { get; set; }

        public MonthModel Month { get; set; } = default!;

        public int MonthId { get; set; }

        public bool Confirmed { get; set; }

        public DateTime Date { get; set; }
    }
}