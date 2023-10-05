namespace WebBlazorEc.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Product>>> GetFeaturedProducts()
        {
            var response = new ServiceResponse<List<Product>>();
            try
            {
                response = new ServiceResponse<List<Product>>
                {
                    Data = await _context.Products
                            .Where(p => p.Featured && p.Visible && !p.Deleted)
                            .Include(p => p.ProductVariants.Where(pv => pv.Visible && !pv.Deleted))
                            .ToListAsync()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        //Lấy danh sách sản phẩm
        public async Task<ServiceResponse<List<Product>>> GetProductAsync()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                            .Where(p => p.Visible && !p.Deleted)
                            .Include(p=>p.ProductVariants.Where(pv => pv.Visible && !pv.Deleted))
                            .ToListAsync()
            };

            return response;
        }

        //Lấy chi tiết sản phẩm
        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var response = new ServiceResponse<Product>();
            //lấy sản phẩm từ csdl với bất đồng bộ, cung cấp id sản phẩm đề tìm
            var product = await _context.Products.
                Include(p => p.ProductVariants.Where(pv => pv.Visible && !pv.Deleted)).
                ThenInclude(t => t.ProductType).
                FirstOrDefaultAsync(p => p.Id == productId && p.Visible && !p.Deleted);

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

        //Lấy danh sách sản phẩm theo danh mục
        public async Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>> {
                Data = await _context.Products
                .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()) &&
                        p.Visible && !p.Deleted)
                .Include(p => p.ProductVariants.Where(pv => pv.Visible && !pv.Deleted))
                .ToListAsync()
            };

            return response;
        }

        //Đề xuất khi gõ phần tìm kiếm sản phẩm
        public async Task<ServiceResponse<List<string>>> GetProductsSearchSuggestions(string searchText)
        {
            var products = await FindProductsBySeacrhText(searchText);

            List<string> result =  new List<string>();
            foreach (var item in products)
            {
                if (item.Title.Contains(searchText,StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(item.Title);
                }

                if (item.Description != null)
                {
                    var punctuation = item.Description.Where(char.IsPunctuation).Distinct().ToArray();
                    var words = item.Description.Split().Select(x=>x.Trim(punctuation));

                    foreach (var word in words)
                    {
                        if (word.Contains(searchText,StringComparison.OrdinalIgnoreCase) &&
                            !result.Contains(word))
                        {
                            result.Add(word);
                        }
                    }
                }
            }
            
            return new ServiceResponse<List<string>> { Data = result };
        }

        //Tìm kiếm sản phẩm 
        public async Task<ServiceResponse<ProductSearchResult>> SearchProducts(string searchText, int page)
        {
            var pageResults = 2f;
            var pageCount = Math.Ceiling((await FindProductsBySeacrhText(searchText)).Count / pageResults);
            var products = await _context.Products
                                    .Where(p => p.Title.ToLower().Contains(searchText.ToLower())
                                        || p.Description.ToLower().Contains(searchText.ToLower()) &&
                                        p.Visible && !p.Deleted)
                                    .Include(p => p.ProductVariants)
                                    .Skip((page - 1) * (int)pageResults)
                                    .Take((int)pageResults)
                                    .ToListAsync();

            var response = new ServiceResponse<ProductSearchResult>
            {
                Data = new ProductSearchResult
                {
                    Products = products,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };
            return response;
        }

        private async Task<List<Product>> FindProductsBySeacrhText(string searchText)
        {
            return await _context.Products
                                    .Where(p => p.Title.ToLower().Contains(searchText.ToLower())
                                        || p.Description.ToLower().Contains(searchText.ToLower()) && 
                                        p.Visible && !p.Deleted)
                                    .Include(p => p.ProductVariants)
                                    .ToListAsync();
        }
    }
}
