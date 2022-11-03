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

        [Required]
        [MaxLength(300)]
        public string LongDescription { get; set; } = string.Empty;

        [Required]
        [MaxLength(75)]
        public string ShortDescription { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; }

        [Required] public virtual Category? Category { get; set; }

        public List<MenuItemIngredient>? Ingredients { get; set; } = new List<MenuItemIngredient>();
    }
}
