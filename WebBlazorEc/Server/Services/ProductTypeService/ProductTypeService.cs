using System.Collections.Generic;
using WebBlazorEc.Shared;

namespace WebBlazorEc.Server.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly DataContext _context;

        public ProductTypeService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<ProductType>>> GetProductTypeAsync()
        {
            var productType = await _context.ProductTypes.ToListAsync();
            return new ServiceResponse<List<ProductType>> { Data = productType };
        }

        public async Task<ServiceResponse<List<ProductType>>> AddProductTypeAsync(ProductType productType)
        {
            productType.Editing = productType.IsNew = false;
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();
            
            return await GetProductTypeAsync();
        }

        public async Task<ServiceResponse<List<ProductType>>> UpdateProductTypeAsync(ProductType productType)
        {
            var dbProType = await _context.ProductTypes.FindAsync(productType.Id);
            if (dbProType == null)
            {
                return new ServiceResponse<List<ProductType>>
                {
                    Message = "Product type not found",
                    Success = false
                };
            }

            dbProType.Name = productType.Name;
            await _context.SaveChangesAsync();

            return await GetProductTypeAsync();
        }
    }
}
