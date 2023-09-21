using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebBlazorEc.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]  //Dạng như 1 bài viết dùng để hiển thị thông tin
        public async Task<ActionResult<ServiceResponse<bool>>> PlaceOrder()
        {
            var result = await _orderService.PlaceOrder();
            return Ok(result);
        }
    }
}
