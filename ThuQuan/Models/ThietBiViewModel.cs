using System.Collections.Generic;

namespace ThuQuan.Models
{
    public class ThietBiViewModel
    {
        public List<ChoNgoi> DanhSachPhong { get; set; }
        public List<MayChieu> DanhSachMayChieu { get; set; }
        public List<MayTinh> DanhSachMayTinh { get; set; }
    }
}
