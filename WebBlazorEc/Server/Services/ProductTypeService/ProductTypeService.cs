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
    }
}
