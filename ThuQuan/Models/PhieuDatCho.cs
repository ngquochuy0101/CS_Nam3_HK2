namespace ThuQuan.Models
{
    public class PhieuDatCho
    {
        public int IdPhieuDatCho { get; set; }
        public int IdChongoi { get; set; }
        public string Ngay { get; set; }
        public string Gio { get; set; }
        public List<string> ChonMayChieu { get; set; }
        public List<string> ChonMayTinh { get; set; }
        public int UserId { get; set; }
        public int NgDatCho { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime Tgian { get; set; }
        public int Status { get; set; }
        public int SoTien { get; set; }
    }


    public class ChiTietPhieuDatCho
    {
        public int IdChiTietPhieuDatCho { get; set; }
        public string IdThietBi { get; set; }
        public int GiaTien { get; set; }
        public int IdPhieuDatCho { get; set; }
    }

}