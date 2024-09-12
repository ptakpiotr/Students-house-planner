using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class ShoppingEntryCreate
    {
        [Required]
        [MinLength(3), MaxLength(255)]
        public string Item { get; set; } = default!;

        public int MonthId { get; set; }

        public bool Confirmed { get; set; } = false;

        public DateTime Date { get; set; }
    }
}