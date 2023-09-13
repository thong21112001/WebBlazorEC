using WebBlazorEc.Server.Services.CartItemService;

namespace WebBlazorEc.Server.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartItemService _cartItemService;

        public CartController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpPost("products")]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> GetCartProducts(List<CartItem> cartItems)
        {
            var result = await _cartItemService.GetCartProducts(cartItems);
            return Ok(result);
        }
    }
}
