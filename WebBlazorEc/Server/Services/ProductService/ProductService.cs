namespace WebBlazorEc.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        //Tạo sp dành cho role admin
        public async Task<ServiceResponse<Product>> CreateProduct(Product product)
        {
            foreach (var productVariant in product.ProductVariants)
            {
                productVariant.ProductType = null;
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ServiceResponse<Product> { Data = product };
        }

        //Cập nhập sp dành cho role admin
        public async Task<ServiceResponse<Product>> UpdateProduct(Product product)
        {
            var dbProduct = await _context.Products.FindAsync(product.Id);

            if (dbProduct == null)
            {
                return new ServiceResponse<Product>
                {
                    Success = false,
                    Message = "Product not found"
                };
            }

            dbProduct.Title = product.Title;
            dbProduct.Description = product.Description;
            dbProduct.ImageUrl = product.ImageUrl;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Visible = product.Visible;
            dbProduct.Featured = product.Featured;

            foreach (var prodVar in product.ProductVariants)
            {
                var dbprodVariant = await _context.ProductVariants
                                .SingleOrDefaultAsync(pv => pv.ProductId == prodVar.ProductId &&
                                pv.ProductTypeId == prodVar.ProductTypeId);
                if (dbprodVariant == null)
                {
                    prodVar.ProductType = null;
                    _context.ProductVariants.Add(prodVar);
                }
                else
                {
                    dbprodVariant.ProductTypeId = prodVar.ProductTypeId;
                    dbprodVariant.Price = prodVar.Price;
                    dbprodVariant.OriginalPrice = prodVar.OriginalPrice;
                    dbprodVariant.Visible = prodVar.Visible;
                    dbprodVariant.Deleted = prodVar.Deleted;
                }
            }

            await _context.SaveChangesAsync();

            return new ServiceResponse<Product> { Data = product };
        }

        //Xoá sp dành cho role admin
        public async Task<ServiceResponse<bool>> DeleteProduct(int productId)
        {
            var dbProduct = await _context.Products.FindAsync(productId);

            if (dbProduct == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Product not found"
                };
            }

            dbProduct.Deleted = true;
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        //Lấy danh sách sp trong admin
        public async Task<ServiceResponse<List<Product>>> GetAdminProducts()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                            .Where(p => !p.Deleted)
                            .Include(p => p.ProductVariants.Where(pv => !pv.Deleted))
                            .ThenInclude(pv => pv.ProductType)
                            .Include(p => p.Images)
                            .ToListAsync()
            };

            return response;
        }

        //Hiển thị sp nổi bật ở user
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
                            .Include(p => p.Images)
                            .ToListAsync()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        //Lấy danh sách tất cảs sản phẩm ở user
        public async Task<ServiceResponse<List<Product>>> GetProductAsync()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                            .Where(p => p.Visible && !p.Deleted)
                            .Include(p => p.ProductVariants.Where(pv => pv.Visible && !pv.Deleted))
                            .Include(p => p.Images)
                            .ToListAsync()
            };

            return response;
        }

        //Lấy chi tiết sản phẩm dành cho admin và user
        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var response = new ServiceResponse<Product>();
            Product product = null;

            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                product = await _context.Products.
                    Include(p => p.ProductVariants.Where(pv => !pv.Deleted)).
                    ThenInclude(t => t.ProductType).
                    Include(p => p.Images).
                    FirstOrDefaultAsync(p => p.Id == productId && p.Visible);
            }
            else
            {
                //lấy sản phẩm từ csdl với bất đồng bộ, cung cấp id sản phẩm đề tìm
                product = await _context.Products.
                    Include(p => p.ProductVariants.Where(pv => pv.Visible && !pv.Deleted)).
                    ThenInclude(t => t.ProductType).
                    Include(p => p.Images).
                    FirstOrDefaultAsync(p => p.Id == productId && p.Visible && !p.Deleted);
            }


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
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()) &&
                        p.Visible && !p.Deleted)
                .Include(p => p.ProductVariants.Where(pv => pv.Visible && !pv.Deleted))
                .Include(p => p.Images)
                .ToListAsync()
            };

            return response;
        }

        //Đề xuất khi gõ phần tìm kiếm sản phẩm
        public async Task<ServiceResponse<List<string>>> GetProductsSearchSuggestions(string searchText)
        {
            var products = await FindProductsBySeacrhText(searchText);

            List<string> result = new List<string>();
            foreach (var item in products)
            {
                if (item.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(item.Title);
                }

                if (item.Description != null)
                {
                    var punctuation = item.Description.Where(char.IsPunctuation).Distinct().ToArray();
                    var words = item.Description.Split().Select(x => x.Trim(punctuation));

                    foreach (var word in words)
                    {
                        if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase) &&
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
                                    .Include(p => p.Images)
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
