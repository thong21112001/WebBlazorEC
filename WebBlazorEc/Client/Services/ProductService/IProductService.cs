namespace WebBlazorEc.Client.Services.ProductService
{
    public interface IProductService
    {
        //Đây là một sự kiện khi mà danh sách sản phẩm thay đổi sẽ gọi cái này
        event Action ProductsChanged;
        List<Product> Products { get; set; }
        List<Product> AdminProducts { get; set; }
        string Message { get; set; }    //Dùng để hiện thị thông báo trong ô tìm kiếm
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        string LastSearchText { get; set; }
        //Thay đổi phương thức lấy tất cả sản phẩm bằng cách thêm categoryUrl
        Task GetProducts(string? categoryUrl = null);
        Task<ServiceResponse<Product>> GetProductAsync(int productId);
        Task SearchProducts(string searchText,int page);
        Task<List<string>> GetProductsSearchSuggestions(string searchText);
        Task GetAdminProducts();
    }
}
