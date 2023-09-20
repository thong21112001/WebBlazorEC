using System.Security.Claims;

namespace WebBlazorEc.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> StoreCartItems(List<CartItem> cartItems)
        {
            var result = await _cartItemService.StoreCartItems(cartItems);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddToCart(CartItem cartItems)    //Them mat hang chu khong phai danh sach
        {
            var result = await _cartItemService.AddToCart(cartItems);
            return Ok(result);
        }

        [HttpPut("update-quantity")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateQuantity(CartItem cartItems)   //Cap nhap san pham
        {
            var result = await _cartItemService.UpdateQuantity(cartItems);
            return Ok(result);
        }

        [HttpGet("count")]
        //Lấy số lượng sp trong giỏ hàng
        public async Task<ActionResult<ServiceResponse<int>>> GetCartItemsCount()
        {
            return await _cartItemService.GetCartItemsCount();
        }

        //Day la controller chinh
        [HttpGet]
        //Lấy số lượng sp trong giỏ hàng
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> GetDbCartProducts()
        {
            var result = await _cartItemService.GetDbCartProducts();

            return Ok(result);
        }
    }
}
