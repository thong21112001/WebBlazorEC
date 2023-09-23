using Stripe.Checkout;

namespace WebBlazorEc.Server.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<Session> CreateCheckoutSession();
    }
}
