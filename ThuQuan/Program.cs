using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using ThuQuan.Models;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình Session
builder.Services.AddDistributedMemoryCache(); // Sử dụng bộ nhớ lưu trữ session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout
    options.Cookie.HttpOnly = true;                 // Cookie chỉ dùng cho HTTP
    options.Cookie.IsEssential = true;              // Cookie cần thiết
});


// ✅ Add MVC + Razor views
builder.Services.AddControllersWithViews();

// ✅ Cấu hình DbContext dùng MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

var app = builder.Build();

// ✅ Middleware Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();     // Phục vụ file tĩnh (wwwroot)
app.UseRouting();

app.UseSession();         // ✅ Kích hoạt Session
app.UseAuthorization();   // ✅ Nên để sau UseSession

// ✅ Routing chính
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// ✅ Tuyến đường tuỳ chỉnh cho login/profile
app.MapControllerRoute(
    name: "login",
    pattern: "login",
    defaults: new { controller = "Login", action = "Login" }
);

app.MapControllerRoute(
    name: "profile",
    pattern: "profile",
    defaults: new { controller = "Profile", action = "Profile" }
);

app.MapControllerRoute(
    name: "editProfile",
    pattern: "edit",
    defaults: new { controller = "Profile", action = "Edit" }
);

app.Run();
