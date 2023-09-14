using Blazored.LocalStorage;

namespace WebBlazorEc.Client.Services.CartItemService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _http;

        public CartService(ILocalStorageService localStorageService, HttpClient http)
        {
            _localStorageService = localStorageService;
            _http = http;
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

        public async Task<List<CartProductResponse>> GetCartProducts()
        {
            var cartItems = await _localStorageService.GetItemAsync<List<CartItem>>("cart");
            var response = await _http.PostAsJsonAsync("api/cart/products", cartItems);
            var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();
            return cartProducts.Data;
        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            //Lấy sp từ bộ nhớ local -> trỏ đến Controller Cart của Server
            var cart = await _localStorageService.GetItemAsync<List<CartItem>>("cart");

            if (cart == null)
                return;

            var cartItem = cart.Find(x => x.ProductId == productId && x.ProductTypeId == productTypeId);

            if (cartItem != null)
            {
                cart.Remove(cartItem);

                //Đặt lại danh sách sản phẩm sau khi xoá
                await _localStorageService.SetItemAsync("cart", cart);
                OnChange.Invoke();
            }
        }
    }
}
