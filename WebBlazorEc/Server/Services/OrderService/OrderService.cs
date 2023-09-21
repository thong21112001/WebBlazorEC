using System.Security.Claims;

namespace WebBlazorEc.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ICartItemService _cartItemService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(DataContext context,
                ICartItemService cartItemService,
                IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _cartItemService = cartItemService;
            _httpContextAccessor = httpContextAccessor;
        }

        //Sử dụng HttpContextAccessor để lấy id của người dùng
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<bool>> PlaceOrder()
        {
            //Lấy danh sách sp trong giỏ hàng
            var products = (await _cartItemService.GetDbCartProducts()).Data;
            decimal totalPrice = 0;

            //Sử dụng vòng lặp để tính tổng số tiền của giỏ hàng
            products.ForEach(product => totalPrice += product.Price * product.Quantity);

            //tạo ms orderItems
            var orderItems = new List<OrderItem>();

            //Sử dụng vòng lặp để thêm dữ liệu vào bảng OrderItems
            products.ForEach(product => orderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity,
                TotalPrice = product.Price * product.Quantity
            }));

            //Tạo mới bảng Order -> thêm dữ liệu vào bảng
            var order = new Order
            {
                UserId = GetUserId(),
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);

            //Xoá các sp trong giỏ hàng với mã userid đăng nhập
            _context.CartItems.RemoveRange(_context.CartItems.Where(ci => ci.UserId == GetUserId()));

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
