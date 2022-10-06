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
        public string Name { get; set; } = string.Empty;

        public int? Stock { get; set; }

        public double? Weight { get; set; }

        public string Allergens { get; set; } = string.Empty;
    }
}
