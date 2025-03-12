using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using ThuQuan.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Thêm DbContext với MySQL
// Thêm DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession(); // Thêm trước UseRouting()
app.UseAuthorization();
app.UseStaticFiles(); // Bật phục vụ tệp tĩnh từ wwwroot
app.MapStaticAssets();




app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapControllerRoute(
    name: "login", // Tên route
    pattern: "login", // Định dạng URL
    defaults: new { controller = "Login", action = "Login" } // Controller và action mặc định
);
app.MapControllerRoute(
    name: "profile",
    pattern: "profile",
    defaults: new { controller = "Profile", action = "profile" }
);
app.MapControllerRoute(
    name: "profile",
    pattern: "Edit",
    defaults: new { controller = "Profile", action = "Edit" }
);




app.Run();
