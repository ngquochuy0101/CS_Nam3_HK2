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

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();  // Ensure that a corresponding view exists (e.g., Views/Login/Register.cshtml)
        }
        [HttpPost]
        public IActionResult Register(string username, string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu không khớp.";
                return View();
            }

            if (_context.User.Any(u => u.UserName == username))
            {
                TempData["ErrorMessage"] = "Tên đăng nhập đã tồn tại.";
                return View();
            }

            var newUser = new User
            {
                UserName = username,
                Email = email,
                Password = password,
                FullName = username,
                DiaChi = "",
                SoDienThoai="0123456789",
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                Quyen = 0, // mặc định là người dùng thường
                Status = 1

            };

            _context.User.Add(newUser);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
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
                TempData["UserName"] = user.UserName;


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
