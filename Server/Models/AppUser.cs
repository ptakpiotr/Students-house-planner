using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }

        public string Login { get; set; } = default!;

        public IList<KitchenEntry> KitchenEntries { get; set; } = new List<KitchenEntry>();

        public IList<BathroomEntry> BathroomEntries { get; set; } = new List<BathroomEntry>();

        public IList<ShoppingEntry> ShoppingEntries { get; set; } = new List<ShoppingEntry>();
    }
}
