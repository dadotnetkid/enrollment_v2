using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [MetadataType(typeof(ItemsMeta))]
    public partial class Items
    {

    }

    public class ItemsMeta
    {
        [Required(ErrorMessage = "Item Name is required")]
        public string ItemName { get; set; }
        [Required(ErrorMessage = "Unit Cost is required")]
        public decimal UnitCost { get; set; }
        [Required(ErrorMessage = "Unit Measure is required")]
        public string UnitMeasureId { get; set; }
    }
}
