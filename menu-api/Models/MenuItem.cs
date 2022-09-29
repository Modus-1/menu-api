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
        public string Name { get; set; }

        public string IconUrl { get; set; }
        public string BannerUrl { get; set; }

        [MaxLength(300)]
        public string LongDescription { get; set; }

        [Required]
        [MaxLength(75)]
        public string ShortDescription { get; set; }

        [Required]
        
        public double Price { get; set; }
        public int CategoryId { get; set; }

        public IList<MenuItem_Ingredient> MenuItems_Ingredients { get; set; }
    }
}
