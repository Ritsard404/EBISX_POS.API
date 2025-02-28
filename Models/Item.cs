using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace EBISX_POS.API.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public required int ItemQTY { get; set; }
        public required decimal ItemPrice { get; set; }
        public required decimal ItemSubTotal { get; set; }
        public required Menu Menu { get; set; }
        public Menu? Drinks { get; set; }
        public Menu? AddOn { get; set; }
        public required Order Order { get; set; }
    }
}
