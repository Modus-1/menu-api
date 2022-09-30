namespace menu_api.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MenuItem")]
    public class MenuItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string IconUrl { get; set; } = string.Empty;

        public string BannerUrl { get; set; } = string.Empty;

        [MaxLength(300)]
        public string LongDescription { get; set; } = string.Empty;

        [Required]
        [MaxLength(75)]
        public string ShortDescription { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; }

        public int CategoryId { get; set; }

        public IList<MenuItem_Ingredient> MenuItemIngredients { get; set; } = new List<MenuItem_Ingredient>();
    }
}
