using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ThuQuan.Models;
using MySql.Data.MySqlClient;

namespace ThuQuan.Controllers;

public class HomeController : Controller

{
    private readonly string connectionString = "server=localhost;database=db_thuquan;uid=root;pwd=;";

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {


        var viewModel = new ThietBiViewModel();

        using (var conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            // Load phòng
            var sqlPhong = "SELECT id_chongoi, so_luong, vi_tri, id_phong FROM cho_ngoi WHERE status = 1";
            using (var cmd = new MySqlCommand(sqlPhong, conn))
            using (var reader = cmd.ExecuteReader())
            {
                viewModel.DanhSachPhong = new List<ChoNgoi>();
                while (reader.Read())
                {
                    viewModel.DanhSachPhong.Add(new ChoNgoi
                    {
                        Id_Chongoi = reader.GetInt32("id_chongoi"),
                        Id_Phong = reader.GetInt32("id_phong"),
                        so_luong = reader.GetInt32("so_luong"),
                        vi_tri = reader.GetInt32("vi_tri")
                    });
                }
            }

            // Load máy chiếu
            var sqlMayChieu = "SELECT id_seri_maychieu, link, gia_tien FROM maychieu WHERE status = 1 AND so_luong > 0";
            using (var cmd = new MySqlCommand(sqlMayChieu, conn))
            using (var reader = cmd.ExecuteReader())
            {
                viewModel.DanhSachMayChieu = new List<MayChieu>();
                while (reader.Read())
                {
                    viewModel.DanhSachMayChieu.Add(new MayChieu
                    {
                        Id_Seri_MayChieu = reader.GetString("id_seri_maychieu"),
                        Link = reader.GetString("link"),
                        Gia_Tien = reader.GetInt32("gia_tien")
                    });
                }
            }

            // Load máy tính
            var sqlMayTinh = "SELECT id_seri_maytinh, link, gia_tien FROM maytinh WHERE status = 1 AND so_luong > 0";
            using (var cmd = new MySqlCommand(sqlMayTinh, conn))
            using (var reader = cmd.ExecuteReader())
            {
                viewModel.DanhSachMayTinh = new List<MayTinh>();
                while (reader.Read())
                {
                    viewModel.DanhSachMayTinh.Add(new MayTinh
                    {
                        Id_Seri_MayTinh = reader.GetString("id_seri_maytinh"),
                        Link = reader.GetString("link"),
                        Gia_Tien = reader.GetInt32("gia_tien")
                    });
                }
            }
        }
        var name = HttpContext.Session.GetString("FullName");
        ViewData["FullName"] = name; // Lưu tên người dùng vào ViewData
        var userId = HttpContext.Session.GetInt32("UserId");
        ViewData["UserId"] = userId;
        Console.WriteLine("UserId_index: " + userId); // Kiểm tra giá trị UserId trong console
        return View(viewModel); // ✅ Trả về đúng ViewModel khớp với @model trong View
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
