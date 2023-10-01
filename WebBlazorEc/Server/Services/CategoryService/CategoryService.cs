namespace WebBlazorEc.Server.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }

        //Lấy 1 danh mục từ id truyền vào
        private async Task<Category> GetCategoryById(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        //Hiển thị danh sách danh mục của admin
        public async Task<ServiceResponse<List<Category>>> GetAdminCategories()
        {
            //Mặc định là Deleted là false và Visible là true
            var cate = await _context.Categories.Where(c => !c.Deleted).ToListAsync();
            return new ServiceResponse<List<Category>>
            {
                Data = cate
            };
        }

        //Thêm danh mục trog admin
        public async Task<ServiceResponse<List<Category>>> AddCategory(Category category)
        {
            category.Editing = category.IsNew = false;
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return await GetAdminCategories();
        }

        //Cập nhập danh mục trog admin
        public async Task<ServiceResponse<List<Category>>> UpdateCategory(Category category)
        {
            var dbCate = await GetCategoryById(category.Id);
            if (dbCate == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "Category not found."
                };
            }

            dbCate.Name = category.Name;
            dbCate.Url = category.Url;
            dbCate.Visible = category.Visible;

            await _context.SaveChangesAsync();

            return await GetAdminCategories();
        }

        //Xoá danh mục trog admin
        public async Task<ServiceResponse<List<Category>>> DeleteCategory(int id)
        {
            Category cate = await GetCategoryById(id);
            if (cate == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "Category not found."
                };
            }

            cate.Deleted = true;    //Mặc định là false
            await _context.SaveChangesAsync();

            return await GetAdminCategories();
        }

        //Hiển thị danh mục bên ngoài User
        public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
        {
            //Mặc định là Deleted là false và Visible là true
            var cate = await _context.Categories.Where(c => !c.Deleted && c.Visible).ToListAsync();
            return new ServiceResponse<List<Category>>
            {
                Data = cate
            };
        }
    }
}
