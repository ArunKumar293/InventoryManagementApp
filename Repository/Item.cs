using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementApp.Repository
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Item Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Item Quantity is Required")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Item Description is Required")]
        public string Description { get; set; }
    }
}
