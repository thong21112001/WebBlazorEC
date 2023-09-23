using Stripe;
using Stripe.Checkout;

namespace WebBlazorEc.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartItemService _cartItemService;
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;

        public PaymentService(ICartItemService cartItemService,
                                IAuthService authService,
                                IOrderService orderService)
        {
            StripeConfiguration.ApiKey = "sk_test_51NY4N7Am7lURX42RAz9C7OCVthNoZLmqmeHLVVKl9ox58xom9VYmQ90vwalpZaEPlzcByS8pVpPmClo0ZiSPY5Yr00dA4LdAc9";
            _cartItemService = cartItemService;
            _authService = authService;
            _orderService = orderService;
        }

        public async Task<Session> CreateCheckoutSession()
        {
            var products = (await _cartItemService.GetDbCartProducts()).Data;
            var lineItems = new List<SessionLineItemOptions>();

            products.ForEach(products => lineItems.Add(new SessionLineItemOptions { 
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = products.Price * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = products.Title,
                        Images = new List<string>
                        {
                            products.ImageUrl
                        }
                    }
                },
                Quantity = products.Quantity
            }));

            var options = new SessionCreateOptions
            {
                CustomerEmail = _authService.GetUserEmail(),
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7120/order-success",
                CancelUrl = "https://localhost:7120/cart",
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }
    }
}
