using System.ComponentModel.DataAnnotations.Schema;

namespace WebBlazorEc.Shared
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [NotMapped] //Không phải là cột trong bảng
        public bool Editing { get; set; } = false;
        [NotMapped] //Không phải là cột trong bảng
        public bool IsNew { get; set; } = false;
    }
}
