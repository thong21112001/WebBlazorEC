﻿namespace WebBlazorEc.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public ProductService(HttpClient http)
        {
            _http = http;
        }

        public List<Product> Products { get; set; } = new List<Product>();

        public event Action ProductsChanged;

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/Product/{productId}");
            return result;
        }

        public async Task GetProducts(string? categoryUrl = null)
        {
            //Nếu như không có categoryUrl thì lấy toàn bộ sản phẩm, ngược lại
            //Thì lấy sản phẩm theo categoryUrl
            var result = categoryUrl == null ?
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/Product") :
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/Product/category/{categoryUrl}");
            
            if (result != null && result.Data != null)
                Products = result.Data;

            //Gọi sự kiện thay đổi sản phẩm thông báo cho các thành phần đăng ký sự kiện này
            ProductsChanged.Invoke();
        }
    }
}
