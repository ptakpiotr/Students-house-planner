using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class MonthModel
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        public string AppIdentifier { get; set; } = default!;

        public int Month { get; set; }

        public int Year { get; set; }

        public IList<KitchenEntry> KitchenEntries { get; set; } = new List<KitchenEntry>();

        public IList<BathroomEntry> BathroomEntries { get; set; } = new List<BathroomEntry>();

        public IList<ShoppingEntry> ShoppingEntries { get; set; } = new List<ShoppingEntry>();

    }
}