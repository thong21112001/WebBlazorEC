namespace WebBlazorEc.Server.Data
{
    public class DataContext : DbContext
    {
        //Gõ tắt ctor -> xong tab
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
    }
}
