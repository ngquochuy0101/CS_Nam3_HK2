using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ThuQuan.Models;

namespace ThuQuan.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

public IActionResult Index()
{
    var userName = TempData["UserName"] as string;
    
    // Gán lại nếu bạn muốn giữ thêm 1 lần nữa (giữ lại TempData sau khi đọc)
    TempData.Keep("UserName");

    ViewBag.UserName = userName;

    return View();
}


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
