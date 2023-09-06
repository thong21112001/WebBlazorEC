namespace WebBlazorEc.Client.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _http;

        public CategoryService(HttpClient http)
        {
            _http = http;
        }

        //Khởi tạo một danh sách danh mục mới là null
        public List<Category> Categories { get; set; } = new List<Category>();

        public async Task GetCategories()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category");
            if (response != null && response.Data != null)
            {
                Categories = response.Data; // gán dữ liệu lấy về từ response là Data cho categories
            }
            
        }
    }
}
