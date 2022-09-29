namespace menu_api.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Ingredient")]
    public class Ingredient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? Stock { get; set; }
        public double? Weight { get; set; }

        public string? Allergens { get; set; }

        public IList<MenuItem_Ingredient> MenuItems_Ingredients { get; set; }
    }
}
