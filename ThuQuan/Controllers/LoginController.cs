using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Data;
using ThuQuan.Models;
using System.Linq;
namespace ThuQuan.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Hiển thị trang đăng nhập
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]

        public IActionResult ForgotPassword(){
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();  // Ensure that a corresponding view exists (e.g., Views/Login/Register.cshtml)
        }

        // POST: Xử lý đăng nhập
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.User
                .FirstOrDefault(u => u.UserName == username && u.Password == password);

            if (user != null && user.Status == 1) // Đăng nhập thành công
            {
                // Lưu thông tin đăng nhập vào Session
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetInt32("UserId", user.UserId);

                // Thông báo thành công
                TempData["SuccessMessage"] = "Đăng nhập thành công!";

                // Chuyển hướng đến trang chủ
                return RedirectToAction("Index", "Home");
            }

            // Đăng nhập thất bại
            TempData["ErrorMessage"] = "Tên đăng nhập hoặc mật khẩu không đúng. Vui lòng nhập lại.";
            return View(); // Trả về trang Login để nhập lại
        }

        // Đăng xuất
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Đăng xuất thành công!";
            return RedirectToAction("Login");
        }
    }
}
