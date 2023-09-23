﻿using Microsoft.AspNetCore.Authorization;

namespace WebBlazorEc.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("checkout"),Authorize]
        public async Task<ActionResult<string>> CreateCheckoutSession()
        {
            var session = await _paymentService.CreateCheckoutSession();
            return Ok(session.Url);
        }
    }
}
