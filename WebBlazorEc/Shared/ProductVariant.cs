using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebBlazorEc.Shared
{
    //Biến thể của sản phẩm
    public class ProductVariant
    {
        //Không dùng id chính của khoá mà dùng khoá ngoại
        //Sử dụng cái này để không bị lặp vòng tròn: ví dụ như không hiện sản phẩm chính mà hãy hiện biến thể của nó
        [JsonIgnore]    
        public Product? Product { get; set; }
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public ProductType? ProductType { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalPrice { get; set; }
        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [NotMapped] //Không phải là cột trong bảng
        public bool Editing { get; set; } = false;
        [NotMapped] //Không phải là cột trong bảng
        public bool IsNew { get; set; } = false;
    }
}
