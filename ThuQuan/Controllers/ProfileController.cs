using Microsoft.AspNetCore.Mvc;

public class ProfileController : Controller
{
    public IActionResult Profile()
    {
        return View("profile"); // Chỉ định view khác nếu cần
    }
    public IActionResult Edit()
    {
        return View("Edit"); // Chỉ định view khác nếu cần
    }

}
