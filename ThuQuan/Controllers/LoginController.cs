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
        public IActionResult ResetPassword()
        {
            return View();
        }

        public IActionResult ForgotPassword(int user_Id)
        {
            ViewBag.UserId = user_Id;
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
                SoDienThoai = "0123456789",
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                Quyen = 0, // mặc định là người dùng thường
                Status = 1

            };


            _context.User.Add(newUser);
            _context.SaveChanges();

            // TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Register"); // Chuyển hướng đến trang đăng nhập sau khi đăng ký thành công
        }


        // POST: Xử lý đăng nhập
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.User
                .FirstOrDefault(u => u.UserName == username && u.Password == password);

            if (user != null && user.Status == 1) // Đăng nhập thành công
            {
                ViewBag.FullName = user.FullName; // hoặc ViewData["FullName"]

                // Lưu thông tin đăng nhập vào Session
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("FullName", user.FullName);
                HttpContext.Session.SetInt32("UserId", user.User_Id); // Lưu ID người dùng vào Session
                var userId = HttpContext.Session.GetInt32("UserId");

                // Kiểm tra nếu không có dữ liệu Session
                if (userId.HasValue)
                {
                    ViewBag.UserId = userId.Value; // Lưu UserId vào ViewBag

                    // Nếu đã có giá trị trong session
                    Console.WriteLine("User đã đăng nhập: " + userId.Value);

                }
                else
                {
                    // Nếu chưa có, tức là chưa đăng nhập
                    Console.WriteLine("Chưa đăng nhập");
                }
                TempData["SuccessMessage"] = "Đăng nhập thành công!";
                return RedirectToAction("Index", "Home");
            }

            // Đăng nhập thất bại
            TempData["ErrorMessage"] = "Tên đăng nhập hoặc mật khẩu không đúng. Vui lòng nhập lại.";
            return View(); // Trả về trang Login để nhập lại
        }

        [HttpPost]
        public IActionResult ForgotPassword(string username, string email, string sodienthoai, string fullname)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(sodienthoai) || string.IsNullOrEmpty(fullname))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập đầy đủ thông tin.";
                return View(new User { UserName = username, Email = email, SoDienThoai = sodienthoai, FullName = fullname });
            }

            var user = _context.User.FirstOrDefault(u =>
                u.UserName == username &&
                u.Email == email &&
                u.SoDienThoai == sodienthoai &&
                u.FullName == fullname);

            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy tài khoản với thông tin cung cấp.";
                return View(new User { UserName = username, Email = email, SoDienThoai = sodienthoai, FullName = fullname });
            }

            Console.WriteLine("user_Id: " + user.User_Id);
            HttpContext.Session.SetInt32("UserId", user.User_Id); // Lưu ID người dùng vào Session
            return RedirectToAction("ResetPassword");


        }

        [HttpPost]
        public IActionResult ResetPassword(string newPassword, string confirmPassword)
        {
            var user_Id = HttpContext.Session.GetInt32("UserId");

            Console.WriteLine("use r_Id_re0: " + user_Id);

            if (newPassword != confirmPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu xác nhận không khớp.";
                return View();
            }

            var user = _context.User.FirstOrDefault(u => u.User_Id == user_Id);
            Console.WriteLine("user_Id_re: " + user_Id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy tài khoản.";
                return View();
            }
            else
            {
                Console.WriteLine("co user: " + user.User_Id);
            }

            // Mã hóa mật khẩu
            user.UpdateAt = DateTime.Now;
            user.Password = newPassword; // hoặc mã hóa nếu có logic mã hóa

            try
            {
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Đặt lại mật khẩu thành công! Bạn có thể đăng nhập.";
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi khi lưu dữ liệu. Vui lòng thử lại.";
                Console.WriteLine(ex.Message);
                return View();
            }
        }



        // Đăng xuất
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            ViewData.Clear();
            Console.WriteLine("đã xóa: " + @ViewData["UserId"]); // In ra UserId để kiểm tra

            TempData["SuccessMessage"] = "Đăng xuất thành công!";
            return RedirectToAction("Login");
        }
    }
}
