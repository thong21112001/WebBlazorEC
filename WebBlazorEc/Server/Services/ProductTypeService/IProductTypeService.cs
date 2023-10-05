namespace WebBlazorEc.Server.Services.ProductTypeService
{
    public interface IProductTypeService
    {
        Task<ServiceResponse<List<ProductType>>> GetProductTypeAsync();
        Task<ServiceResponse<List<ProductType>>> AddProductTypeAsync(ProductType productType);
        Task<ServiceResponse<List<ProductType>>> UpdateProductTypeAsync(ProductType productType);
    }
}
