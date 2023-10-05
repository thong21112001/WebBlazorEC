namespace WebBlazorEc.Client.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly HttpClient _http;

        public ProductTypeService(HttpClient http)
        {
            _http = http;
        }

        public List<ProductType> ProductTypes { get; set; }

        public event Action OnChange;

        public async Task GetProductTypeAsync()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/ProductType");
            ProductTypes = result.Data;
        }

        public ProductType CreateNewProductType()
        {
            var newProductType = new ProductType { IsNew = true, Editing = true};
            ProductTypes.Add(newProductType);
            OnChange.Invoke();
            return newProductType;
        }
        public async Task AddProductTypeAsync(ProductType productType)
        {
            var response = await _http.PostAsJsonAsync("api/ProductType", productType);
            ProductTypes = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
            OnChange.Invoke();
        }

        public async Task UpdateProductTypeAsync(ProductType productType)
        {
            var response = await _http.PutAsJsonAsync("api/ProductType", productType);
            ProductTypes = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
            OnChange.Invoke();
        }
    }
}
