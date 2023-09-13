﻿namespace WebBlazorEc.Server.Services.CartItemService
{
    public class CartItemService : ICartItemService
    {
        private readonly DataContext _context;

        public CartItemService(DataContext context)
        {
            _context = context;
        }

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
                    ProductTypeId = productVariant.ProductTypeId
                };
                result.Data.Add(cartProduct);
            }

            return result;
        }
    }
}
