using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBlazorEc.Shared
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        //Xoá column Price đi vì trong biến thể của nó đã có Price và OriginalPrice
        public bool Featured { get; set; } = false; //Sản phẩm hot hay không
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [NotMapped] //Không phải là cột trong bảng
        public bool Editing { get; set; } = false;
        [NotMapped] //Không phải là cột trong bảng
        public bool IsNew { get; set; } = false;
    }
}
