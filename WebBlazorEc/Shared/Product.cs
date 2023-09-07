using System.ComponentModel.DataAnnotations.Schema;

namespace WebBlazorEc.Shared
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        //Xoá column Price đi vì trong biến thể của nó đã có Price và OriginalPrice
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    }
}
