
using BilgeShop.Business.Managers;
using BilgeShop.Business.Services;
using BilgeShop.Data.Context;
using BilgeShop.Data.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BilgeShopContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(SqlRepository<>));
// IRepository'e dependency injection yapıldığında, SqlRepository nesnesi gönder.

builder.Services.AddScoped<IUserService, UserManager>();
// AddScoped -> Her istek için yeni bir kopya
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IProductService, ProductManager>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/");
    options.LogoutPath = new PathString("/");
    options.AccessDeniedPath = new PathString("/");
    // giriş - çıkış - erişim reddi durumlarında ana sayfaya yönlendiriyorum.
});



var app = builder.Build();

app.UseStaticFiles(); // wwwroot kullanılacak.

app.UseAuthentication();
app.UseAuthorization();
// Kimlik belirleme ve yetkilendirme i�lemleri i�in gerekli.

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=dashboard}/{action=index}/{id?}"
    );

// default route her zaman en altta.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}"
    );



// app.MapDefaultControllerRoute(); üsttekinin kısa yolu.


app.Run();
