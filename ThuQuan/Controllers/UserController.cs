using Microsoft.AspNetCore.Mvc;

public class UserController : Controller
{
    public IActionResult Profile()
    {
        // Kiểm tra người dùng đã đăng nhập chưa

        return View();
    }
}
