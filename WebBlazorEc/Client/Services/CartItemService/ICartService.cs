namespace WebBlazorEc.Client.Services.CartItemService
{
    public interface ICartService
    {
        event Action OnChange;

        Task AddToCart(CartItem cartItem);  //Thêm sp vào giỏ hàng
        Task<List<CartItem>> GetCartItems();    //Tạo một danh sách để lưu trữ sp
    }
}
