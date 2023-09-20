﻿namespace WebBlazorEc.Server.Services.CartItemService
{
    public interface ICartItemService
    {
        Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<CartItem> cartItems);
        Task<ServiceResponse<List<CartProductResponse>>> StoreCartItems(List<CartItem> cartItems,int userId);
    }
}
