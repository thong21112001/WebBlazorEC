using WebBlazorEc.Shared;

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
        public List<Category> AdminCategories { get; set; } = new List<Category>(); //Này dành cho bên admin

        public event Action OnChange;


        public async Task GetAdminCategories()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category/admin");
            if (response != null && response.Data != null)
            {
                AdminCategories = response.Data; // gán dữ liệu lấy về từ response là Data cho categories
            }
        }

        public async Task GetCategories()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category");
            if (response != null && response.Data != null)
            {
                Categories = response.Data; // gán dữ liệu lấy về từ response là Data cho categories
            }
        }

        public async Task AddCategory(Category category)
        {
            var response = await _http.PostAsJsonAsync("api/category/admin", category);
            AdminCategories = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            await GetCategories();
            OnChange.Invoke();
        }

        public async Task UpdateCategory(Category category)
        {
            var response = await _http.PutAsJsonAsync("api/category/admin", category);
            AdminCategories = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            await GetCategories();
            OnChange.Invoke();
        }

        public async Task DeleteCategory(int id)
        {
            var response = await _http.DeleteAsync($"api/category/admin/{id}");
            AdminCategories = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;
            await GetCategories();
            OnChange.Invoke();
        }

        public Category CreateNewCategory()
        {
            var newCate = new Category { IsNew = true, Editing = true};
            AdminCategories.Add(newCate);
            OnChange.Invoke();
            return newCate;
        }
    }
}
