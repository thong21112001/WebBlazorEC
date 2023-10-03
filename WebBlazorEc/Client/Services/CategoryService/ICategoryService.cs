namespace WebBlazorEc.Client.Services.CategoryService
{
    public interface ICategoryService
    {
        event Action OnChange;

        List<Category> Categories { get; set; }
        Task GetCategories();
        List<Category> AdminCategories { get; set; }
        Task GetAdminCategories();
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int id);
        Category CreateNewCategory();
    }
}
