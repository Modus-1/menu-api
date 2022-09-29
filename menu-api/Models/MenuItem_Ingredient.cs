namespace menu_api.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MenuItem_Ingredient")]
    public class MenuItem_Ingredient
    {
        public Guid MenuItemId { get; set; }
        public MenuItem menuItem { get; set; }

        public Guid IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
