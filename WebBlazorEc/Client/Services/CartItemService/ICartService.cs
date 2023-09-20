﻿namespace WebBlazorEc.Client.Services.CartItemService
{
    public interface ICartService
    {
        event Action OnChange;

        Task AddToCart(CartItem cartItem);  //Thêm sp vào giỏ hàng
        Task<List<CartProductResponse>> GetCartProducts();    //Lay danh sach sp 
        Task RemoveProductFromCart(int productId, int productTypeId);
        Task UpdateQuantity(CartProductResponse product);
        Task StoreCartItems(bool emptyLocalCart);
        Task GetCartItemsCount();
    }
}
