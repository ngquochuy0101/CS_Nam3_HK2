using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ThuQuan.Models;
using System;

namespace ThuQuan.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connectionString = "server=localhost;database=db_thuquan;uid=root;pwd=;";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Action hiện có để hiển thị form
        public IActionResult Index()
        {
            // Giữ nguyên mã hiện có của Index...
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
            ViewData["FullName"] = name;
            var userId = HttpContext.Session.GetInt32("UserId");
            ViewData["UserId"] = userId;
            return View(viewModel);
        }

        // Action mới để xử lý đặt lịch
        [HttpPost]
        public IActionResult DatLich([FromBody] PhieuDatCho model)
        {
            var name = HttpContext.Session.GetString("FullName");
            ViewData["FullName"] = name;
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                   

                    // 1. Kiểm tra chỗ ngồi đã được đặt chưa
                    var sqlCheckChongoi = @"
                        SELECT COUNT(*) 
                        FROM phieu_datcho 
                        WHERE id_chongoi = @idChongoi 
                        AND tgian = @tgian 
                        AND status = 1";
                    using (var cmd = new MySqlCommand(sqlCheckChongoi, conn))
                    {
                        cmd.Parameters.AddWithValue("@idChongoi", model.IdChongoi);
                        cmd.Parameters.AddWithValue("@tgian", DateTime.Parse($"{model.Ngay} {model.Gio}"));
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count > 0)
                        {
                            return Json(new { success = false, message = "Chỗ ngồi này đã được đặt tại thời gian bạn chọn." });
                        }
                    }

                    // 2. Kiểm tra số lượng máy chiếu
                    if (model.ChonMayChieu != null && model.ChonMayChieu.Any())
                    {
                        var sqlCheckMayChieu = @"
                    SELECT id_seri_maychieu, so_luong 
                    FROM maychieu 
                    WHERE id_seri_maychieu IN (";
                        for (int i = 0; i < model.ChonMayChieu.Count; i++)
                        {
                            sqlCheckMayChieu += $"@mc{i},";
                        }
                        sqlCheckMayChieu = sqlCheckMayChieu.TrimEnd(',') + ")";
                        using (var cmd = new MySqlCommand(sqlCheckMayChieu, conn))
                        {
                            for (int i = 0; i < model.ChonMayChieu.Count; i++)
                            {
                                cmd.Parameters.AddWithValue($"@mc{i}", model.ChonMayChieu[i]);
                            }
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int soLuong = reader.GetInt32("so_luong");
                                    if (soLuong <= 0)
                                    {
                                        return Json(new { success = false, message = $"Máy chiếu {reader.GetString("id_seri_maychieu")} đã hết số lượng." });
                                    }
                                }
                            }
                        }
                    }

                    // 3. Kiểm tra số lượng máy tính
                    if (model.ChonMayTinh != null && model.ChonMayTinh.Any())
                    {
                        var sqlCheckMayTinh = @"
                    SELECT id_seri_maytinh, so_luong 
                    FROM maytinh 
                    WHERE id_seri_maytinh IN (";
                        for (int i = 0; i < model.ChonMayTinh.Count; i++)
                        {
                            sqlCheckMayTinh += $"@mt{i},";
                        }
                        sqlCheckMayTinh = sqlCheckMayTinh.TrimEnd(',') + ")";
                        using (var cmd = new MySqlCommand(sqlCheckMayTinh, conn))
                        {
                            for (int i = 0; i < model.ChonMayTinh.Count; i++)
                            {
                                cmd.Parameters.AddWithValue($"@mt{i}", model.ChonMayTinh[i]);
                            }
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int soLuong = reader.GetInt32("so_luong");
                                    if (soLuong <= 0)
                                    {
                                        return Json(new { success = false, message = $"Máy tính {reader.GetString("id_seri_maytinh")} đã hết số lượng." });
                                    }
                                }
                            }
                        }
                    }

                    // 4. Tính tổng tiền từ máy chiếu và thu thập giá tiền từng máy chiếu
                    int tongTien = 0;
                    var mayChieuGiaTien = new Dictionary<string, int>();
                    if (model.ChonMayChieu != null && model.ChonMayChieu.Any())
                    {
                        var sqlMayChieu = "SELECT id_seri_maychieu, gia_tien FROM maychieu WHERE id_seri_maychieu IN (";
                        for (int i = 0; i < model.ChonMayChieu.Count; i++)
                        {
                            sqlMayChieu += $"@mc{i},";
                        }
                        sqlMayChieu = sqlMayChieu.TrimEnd(',') + ")";
                        using (var cmd = new MySqlCommand(sqlMayChieu, conn))
                        {
                            for (int i = 0; i < model.ChonMayChieu.Count; i++)
                            {
                                cmd.Parameters.AddWithValue($"@mc{i}", model.ChonMayChieu[i]);
                            }
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string id = reader.GetString("id_seri_maychieu");
                                    int giaTien = reader.GetInt32("gia_tien");
                                    mayChieuGiaTien[id] = giaTien;
                                    tongTien += giaTien;
                                }
                            }
                        }
                    }

                    // 5. Tính tổng tiền từ máy tính và thu thập giá tiền từng máy tính
                    var mayTinhGiaTien = new Dictionary<string, int>();
                    if (model.ChonMayTinh != null && model.ChonMayTinh.Any())
                    {
                        var sqlMayTinh = "SELECT id_seri_maytinh, gia_tien FROM maytinh WHERE id_seri_maytinh IN (";
                        for (int i = 0; i < model.ChonMayTinh.Count; i++)
                        {
                            sqlMayTinh += $"@mt{i},";
                        }
                        sqlMayTinh = sqlMayTinh.TrimEnd(',') + ")";
                        using (var cmd = new MySqlCommand(sqlMayTinh, conn))
                        {
                            for (int i = 0; i < model.ChonMayTinh.Count; i++)
                            {
                                cmd.Parameters.AddWithValue($"@mt{i}", model.ChonMayTinh[i]);
                            }
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string id = reader.GetString("id_seri_maytinh");
                                    int giaTien = reader.GetInt32("gia_tien");
                                    mayTinhGiaTien[id] = giaTien;
                                    tongTien += giaTien;
                                }
                            }
                        }
                    }

                    // 6. Chèn dữ liệu vào bảng phieu_datcho
                    int idPhieuDatCho;
                    var sqlGetMaxId = "SELECT IFNULL(MAX(id_phieudatcho), 0) + 1 FROM phieu_datcho";
                    using (var cmd = new MySqlCommand(sqlGetMaxId, conn))
                    {
                        idPhieuDatCho = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    var sqlInsert = @"
                INSERT INTO phieu_datcho (id_phieudatcho, ngdatcho, id_chongoi, create_at, tgian, status, so_tien)
                VALUES (@idPhieuDatCho, @userId, @idChongoi, @createAt, @tgian, @status, @soTien)";
                    using (var cmd = new MySqlCommand(sqlInsert, conn))
                    {
                        cmd.Parameters.AddWithValue("@idPhieuDatCho", idPhieuDatCho);
                        cmd.Parameters.AddWithValue("@userId", model.UserId);
                        cmd.Parameters.AddWithValue("@idChongoi", model.IdChongoi);
                        cmd.Parameters.AddWithValue("@createAt", DateTime.UtcNow);
                        cmd.Parameters.AddWithValue("@tgian", DateTime.Parse($"{model.Ngay} {model.Gio}"));
                        cmd.Parameters.AddWithValue("@status", 1);
                        cmd.Parameters.AddWithValue("@soTien", tongTien);
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            return Json(new { success = false, message = $"Lỗi khi chèn phiếu đặt chỗ: {ex.Message}, idChongoi: {model.IdChongoi}" });
                        }
                    }

                    // 7. Chèn dữ liệu vào bảng chitiet_phieudatcho cho máy chiếu
                    if (model.ChonMayChieu != null && model.ChonMayChieu.Any())
                    {
                        foreach (var idMayChieu in model.ChonMayChieu)
                        {
                            var sqlInsertChiTiet = @"
                        INSERT INTO chitiet_phieudatcho (id_chitiet_phieudatcho, id_thietbi, gia_tien, id_phieudatcho)
                        VALUES (@idChiTiet, @idThietBi, @giaTien, @idPhieuDatCho)";
                            using (var cmd = new MySqlCommand(sqlInsertChiTiet, conn))
                            {
                                int idChiTiet;
                                var sqlGetMaxId1 = "SELECT IFNULL(MAX(id_chitiet_phieudatcho), 0) + 1 FROM chitiet_phieudatcho";
                                using (var cmd1 = new MySqlCommand(sqlGetMaxId1, conn))
                                {
                                    idChiTiet = Convert.ToInt32(cmd1.ExecuteScalar());
                                }
                                cmd.Parameters.AddWithValue("@idChiTiet", idChiTiet);
                                cmd.Parameters.AddWithValue("@idThietBi", idMayChieu);
                                cmd.Parameters.AddWithValue("@giaTien", mayChieuGiaTien[idMayChieu]);
                                cmd.Parameters.AddWithValue("@idPhieuDatCho", idPhieuDatCho);
                                try
                                {
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    return Json(new { success = false, message = $"Lỗi khi chèn chi tiết máy chiếu: {ex.Message}, idThietBi: {idMayChieu}" });
                                }
                            }
                        }
                    }

                    // 8. Chèn dữ liệu vào bảng chitiet_phieudatcho cho máy tính
                    if (model.ChonMayTinh != null && model.ChonMayTinh.Any())
                    {
                        foreach (var idMayTinh in model.ChonMayTinh)
                        {
                            var sqlInsertChiTiet = @"
                        INSERT INTO chitiet_phieudatcho (id_chitiet_phieudatcho, id_thietbi, gia_tien, id_phieudatcho)
                        VALUES (@idChiTiet, @idThietBi, @giaTien, @idPhieuDatCho)";
                            using (var cmd = new MySqlCommand(sqlInsertChiTiet, conn))
                            {
                                int idChiTiet;
                                var sqlGetMaxId1 = "SELECT IFNULL(MAX(id_chitiet_phieudatcho), 0) + 1 FROM chitiet_phieudatcho";
                                using (var cmd1 = new MySqlCommand(sqlGetMaxId1, conn))
                                {
                                    idChiTiet = Convert.ToInt32(cmd1.ExecuteScalar());
                                }
                                cmd.Parameters.AddWithValue("@idChiTiet", idChiTiet);
                                cmd.Parameters.AddWithValue("@idThietBi", idMayTinh);
                                cmd.Parameters.AddWithValue("@giaTien", mayTinhGiaTien[idMayTinh]);
                                cmd.Parameters.AddWithValue("@idPhieuDatCho", idPhieuDatCho);
                                try
                                {
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    return Json(new { success = false, message = $"Lỗi khi chèn chi tiết máy tính: {ex.Message}, idThietBi: {idMayTinh}" });
                                }
                            }
                        }
                    }

                    // 9. Cập nhật số lượng máy chiếu
                    if (model.ChonMayChieu != null && model.ChonMayChieu.Any())
                    {
                        var sqlUpdateMayChieu = "UPDATE maychieu SET so_luong = so_luong - 1 WHERE id_seri_maychieu IN (";
                        for (int i = 0; i < model.ChonMayChieu.Count; i++)
                        {
                            sqlUpdateMayChieu += $"@mc{i},";
                        }
                        sqlUpdateMayChieu = sqlUpdateMayChieu.TrimEnd(',') + ")";
                        using (var cmd = new MySqlCommand(sqlUpdateMayChieu, conn))
                        {
                            for (int i = 0; i < model.ChonMayChieu.Count; i++)
                            {
                                cmd.Parameters.AddWithValue($"@mc{i}", model.ChonMayChieu[i]);
                            }
                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                return Json(new { success = false, message = $"Lỗi khi cập nhật số lượng máy chiếu: {ex.Message}" });
                            }
                        }
                    }

                    // 10. Cập nhật số lượng máy tính
                    if (model.ChonMayTinh != null && model.ChonMayTinh.Any())
                    {
                        var sqlUpdateMayTinh = "UPDATE maytinh SET so_luong = so_luong - 1 WHERE id_seri_maytinh IN (";
                        for (int i = 0; i < model.ChonMayTinh.Count; i++)
                        {
                            sqlUpdateMayTinh += $"@mt{i},";
                        }
                        sqlUpdateMayTinh = sqlUpdateMayTinh.TrimEnd(',') + ")";
                        using (var cmd = new MySqlCommand(sqlUpdateMayTinh, conn))
                        {
                            for (int i = 0; i < model.ChonMayTinh.Count; i++)
                            {
                                cmd.Parameters.AddWithValue($"@mt{i}", model.ChonMayTinh[i]);
                            }
                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                return Json(new { success = false, message = $"Lỗi khi cập nhật số lượng máy tính: {ex.Message}" });
                            }
                        }
                    }

                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi chung: {ex.Message}, StackTrace: {ex.StackTrace}" });
            }
        }
        // Action hiển thị lịch sử đặt chỗ
        public IActionResult LichSuDatCho()
        {
            var name = HttpContext.Session.GetString("FullName");
            ViewData["FullName"] = name;
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }

            var ph = new List<PhieuDatCho>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var sql = @"
                    SELECT id_phieudatcho, ngdatcho, id_chongoi, create_at, tgian, status, so_tien 
                    FROM phieu_datcho 
                    WHERE ngdatcho = @userId";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId.Value);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ph.Add(new PhieuDatCho
                            {
                                IdPhieuDatCho = reader.GetInt32("id_phieudatcho"),
                                UserId = reader.GetInt32("ngdatcho"),
                                IdChongoi = reader.GetInt32("id_chongoi"),
                                CreateAt = reader.GetDateTime("create_at"),
                                Tgian = reader.GetDateTime("tgian"),
                                Status = reader.GetInt32("status"),
                                SoTien = reader.GetInt32("so_tien")
                            });
                        }
                    }
                }
            }
            return View(ph);
        }

        // Action hiển thị chi tiết phiếu đặt chỗ
        public IActionResult ChiTietPhieuDatCho(string id)
        {
            var name = HttpContext.Session.GetString("FullName");
            ViewData["FullName"] = name;
            var chiTietList = new List<ChiTietPhieuDatCho>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var sql = @"
                    SELECT id_chitiet_phieudatcho, id_thietbi, gia_tien, id_phieudatcho 
                    FROM chitiet_phieudatcho 
                    WHERE id_phieudatcho = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            chiTietList.Add(new ChiTietPhieuDatCho
                            {
                                IdChiTietPhieuDatCho = reader.GetInt32("id_chitiet_phieudatcho"),
                                IdThietBi = reader.GetString("id_thietbi"),
                                GiaTien = reader.GetInt32("gia_tien"),
                                IdPhieuDatCho = reader.GetInt32("id_phieudatcho")
                            });
                        }
                    }
                }
            }
            return View(chiTietList);
        }
    }
}