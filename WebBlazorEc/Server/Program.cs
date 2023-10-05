global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using WebBlazorEc.Server.Data;
global using WebBlazorEc.Server.Services.AuthService;
global using WebBlazorEc.Server.Services.CartItemService;
global using WebBlazorEc.Server.Services.CategoryService;
global using WebBlazorEc.Server.Services.OrderService;
global using WebBlazorEc.Server.Services.PaymentService;
global using WebBlazorEc.Server.Services.ProductService;
global using WebBlazorEc.Server.Services.AddressService;
global using WebBlazorEc.Server.Services.ProductTypeService;
global using WebBlazorEc.Shared;    // khi sử dụng global thì nó sẽ tự động add vào các class
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<ICartItemService,CartItemService>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new 
                SymmetricSecurityKey(System.Text.Encoding.UTF8.
                    GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
               ValidateIssuer = false,
               ValidateAudience = false
           };
       });

//builder.Services.AddHostedService<StripeWebhookListener>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
