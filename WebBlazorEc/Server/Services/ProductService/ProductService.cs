namespace WebBlazorEc.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductAsync()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products.ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var response = new ServiceResponse<Product>();
            //lấy sản phẩm từ csdl với bất đồng bộ, cung cấp id sản phẩm đề tìm
            var product = await _context.Products.FindAsync(productId);

            if (product == null)    //nếu không có sản phẩm
            {
                response.Success = false;
                response.Message = "Sorry, but this produt does not exist.";
            }
            else //Ngược lại
            {
                response.Data = product;
            }

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>> {
                Data = await _context.Products
                .Where(x => x.Category.Url.ToLower().Equals(categoryUrl.ToLower()))
                .ToListAsync()
            };

            return response;
        }
    }
}
