using Blazored.LocalStorage;

namespace WebBlazorEc.Client.Services.CartItemService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorageService;

        public CartService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public event Action OnChange;
        
        //Thêm sp vào giỏ hàng
        public async Task AddToCart(CartItem cartItem)
        {
            var cart = await _localStorageService.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)   //Nếu cart null thì tạo một cart mới
            {
                cart = new List<CartItem>();
            }
            //Thêm sp vào CartItem
            cart.Add(cartItem);

            //sử dụng bộ nhớ cục bộ để đặt thời gian
            await _localStorageService.SetItemAsync("cart", cart);
            OnChange.Invoke();  //Sự thay đổi
        }

        //Lấy danh sách sp của giỏ hàng
        public async Task<List<CartItem>> GetCartItems()
        {
            var cart = await _localStorageService.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
            }
            return cart;
        }
    }
}
