using Microsoft.AspNetCore.Mvc;

namespace WebBlazorEc.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ProductController(DataContext data)
        {
            _dataContext = data;
        }

        [HttpGet]
        //Sửa chỗ này để get dữ liệu từ swaggerUI
        public async Task<ActionResult<List<Product>>> GetProduct()
        {
            var products = await _dataContext.Products.ToListAsync();
            return Ok(products);
        }
    }
}
