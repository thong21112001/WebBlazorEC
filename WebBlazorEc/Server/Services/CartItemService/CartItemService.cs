using System.Security.Claims;

namespace WebBlazorEc.Server.Services.CartItemService
{
    public class CartItemService : ICartItemService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartItemService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        //Sử dụng HttpContextAccessor để lấy id của người dùng
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<CartItem> cartItems)
        {
            //Taọ một phản hồi mới cho CartProductResponse
            var result = new ServiceResponse<List<CartProductResponse>>
            {
                //Dữ liệu cũng được tạo mới từ danh sách CartProductResponse
                Data = new List<CartProductResponse>()
            };  

            foreach (var item in cartItems)
            {
                var product = await _context.Products.Where(p=>p.Id == item.ProductId).FirstOrDefaultAsync();

                if (product == null)
                {
                    continue;
                }

                var productVariant = await _context.ProductVariants.Where(v => v.ProductId == item.ProductId
                                                                    && v.ProductTypeId == item.ProductTypeId).Include(v=>v.ProductType)
                                                                    .FirstOrDefaultAsync();

                if (productVariant == null)
                {
                    continue;
                }

                var cartProduct = new CartProductResponse
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    ImageUrl = product.ImageUrl,
                    Price = productVariant.Price,
                    ProductType = productVariant.ProductType.Name,
                    ProductTypeId = productVariant.ProductTypeId,
                    Quantity = item.Quantity    //Thêm số lượng sp vào này của bài 47
                };
                result.Data.Add(cartProduct);
            }

            return result;
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> StoreCartItems(List<CartItem> cartItems)
        {
            cartItems.ForEach(cartItems => cartItems.UserId = GetUserId());
            _context.CartItems.AddRange(cartItems);
            await _context.SaveChangesAsync();

            return  await GetDbCartProducts();
        }

        public async Task<ServiceResponse<int>> GetCartItemsCount()
        {
            var count = (await _context.CartItems.Where(ci => ci.UserId == GetUserId()).ToListAsync()).Count;
            return new ServiceResponse<int>
            {
                Data = count
            };
        }

        //Lay danh sach gio hang trong Db
        public async Task<ServiceResponse<List<CartProductResponse>>> GetDbCartProducts()
        {
            return await GetCartProducts(await _context.CartItems.Where(ci=>ci.UserId == GetUserId()).ToListAsync());
        }
    }
}
