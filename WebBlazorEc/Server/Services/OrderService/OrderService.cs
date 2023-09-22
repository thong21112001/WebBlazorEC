using System.Security.Claims;

namespace WebBlazorEc.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ICartItemService _cartItemService;
        private readonly IAuthService _authService;

        public OrderService(DataContext context,
                ICartItemService cartItemService,
                IAuthService authService)
        {
            _context = context;
            _cartItemService = cartItemService;
            _authService = authService;
        }

        public async Task<ServiceResponse<List<OrderOverviewResponse>>> GetOrders()
        {
            var response = new ServiceResponse<List<OrderOverviewResponse>>();

            var orders = await _context.Orders.Include(o => o.OrderItems)
                                              .ThenInclude(oi => oi.Product)
                                              .Where(o => o.UserId == _authService.GetUserId())
                                              .OrderByDescending(o => o.OrderDate)
                                              .ToListAsync();

            var orderResponse =  new List<OrderOverviewResponse>();
            orders.ForEach(o => orderResponse.Add(new OrderOverviewResponse
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                Product = o.OrderItems.Count > 1 ? $"{o.OrderItems.First().Product.Title} and" +
                                                   $"{o.OrderItems.Count - 1} more..." :
                                                   o.OrderItems.First().Product.Title,
                ProductImageUrl = o.OrderItems.First().Product.ImageUrl
            }));

            response.Data = orderResponse;

            return response;
        }

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
                UserId = _authService.GetUserId(),
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);

            //Xoá các sp trong giỏ hàng với mã userid đăng nhập
            _context.CartItems.RemoveRange(_context.CartItems.Where(ci => ci.UserId == _authService.GetUserId()));

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
