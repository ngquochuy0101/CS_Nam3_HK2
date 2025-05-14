using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ThuQuan.Models

{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Tự động tăng        [Column("user_id")]
        public int User_Id { get; set; }

        [Column("user_name")]
        public string? UserName { get; set; }

        [Column("password")]
        public string? Password { get; set; }

        [Column("full_name")]
        public string? FullName { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("sodienthoai")]
        public string? SoDienThoai { get; set; }

        [Column("diachi")]
        public string? DiaChi { get; set; }

        [Column("quyen")]
        public int Quyen { get; set; }

        [Column("status")]
        public int Status { get; set; }
    }
}