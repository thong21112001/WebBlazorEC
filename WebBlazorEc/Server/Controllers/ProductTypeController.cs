using Microsoft.AspNetCore.Authorization;

namespace WebBlazorEc.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeService _productTypeService;

        public ProductTypeController(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> GetProductTypeAsync()
        {
            var response = await _productTypeService.GetProductTypeAsync();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> AddProductTypeAsync(ProductType productType)
        {
            var response = await _productTypeService.AddProductTypeAsync(productType);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<ProductType>>>> UpdateProductTypeAsync(ProductType productType)
        {
            var response = await _productTypeService.UpdateProductTypeAsync(productType);
            return Ok(response);
        }
    }
}
