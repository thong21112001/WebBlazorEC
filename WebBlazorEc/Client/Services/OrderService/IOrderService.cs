namespace WebBlazorEc.Client.Services.OrderService
{
    public interface IOrderService
    {
        Task<string> PlaceOrder();  //Để trả về chuỗi url sau khi thanh toán xong
        Task<List<OrderOverviewResponse>> GetOrders();
        Task<OrderDetailsResponse> GetOrderDetails(int orderId);
    }
}
