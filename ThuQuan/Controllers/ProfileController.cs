using Microsoft.AspNetCore.Mvc;
using ThuQuan.Models;
using MyMvcApp.Data;

namespace ThuQuan.Controllers.Mvc;

public class ProfileController : Controller
{
    private readonly AppDbContext _context;

    public ProfileController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Profile()
    {

        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId.HasValue)
        {
            var user = _context.User.FirstOrDefault(u => u.User_Id == userId.Value);

            if (user != null)
            {
                ViewBag.FullName = user.FullName;
                ViewBag.Email = user.Email;
                ViewBag.SoDienThoai = user.SoDienThoai;

                return View(); // Hiển thị View Profile.cshtml
            }

            TempData["ErrorMessage"] = "Không tìm thấy thông tin người dùng.";
            return RedirectToAction("Login", "Login");
        }
        return View(); // Hiển thị View Profile.cshtml


    }
    [HttpGet]
    public IActionResult Edit()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId.HasValue)
        {
            var user = _context.User.FirstOrDefault(u => u.User_Id == userId.Value);
            if (user != null)
            {
                ViewBag.FullName = user.FullName;
                ViewBag.Email = user.Email;
                ViewBag.SoDienThoai = user.SoDienThoai;
                return View();
            }
        }

        TempData["ErrorMessage"] = "Vui lòng đăng nhập để chỉnh sửa thông tin!";
        return RedirectToAction("Login", "Login");
    }

    [HttpPost]
    public IActionResult Edit(string name, string email, string sdt, string password)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId.HasValue)
        {
            var user = _context.User.FirstOrDefault(u => u.User_Id == userId.Value);

            if (user != null)
            {
                user.FullName = name;
                user.Email = email;
                user.SoDienThoai = sdt;
                ViewBag.FullName = name;
                ViewBag.Email = email;
                ViewBag.SoDienThoai = sdt;
                if (!string.IsNullOrEmpty(password))
                {
                    user.Password = password;
                }

                _context.SaveChanges();

                TempData["SuccessMessage"] = "Thông tin đã được cập nhật thành công!";
                return View();
            }

            TempData["ErrorMessage"] = "Vui lòng đăng nhập để sửa thông tin!";
            return RedirectToAction("Login", "Login");
        }

        return View();
    }

}
