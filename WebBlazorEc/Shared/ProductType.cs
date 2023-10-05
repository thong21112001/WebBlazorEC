using System.ComponentModel.DataAnnotations.Schema;

namespace WebBlazorEc.Shared
{
    //Bảng loại sản phẩm
    public class ProductType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [NotMapped] //Không phải là cột trong bảng
        public bool Editing { get; set; } = false;
        [NotMapped] //Không phải là cột trong bảng
        public bool IsNew { get; set; } = false;
    }
}
