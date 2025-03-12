using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginMVC.Models
{
    public class User
    {
        [Key] // Đánh dấu UserId là khóa chính
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("user_name")]
        public string UserName { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("full_name")]
        public string FullName { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("sodienthoai")]
        public string SoDienThoai { get; set; }

        [Column("diachi")]
        public string DiaChi { get; set; }

        [Column("create_at")]
        public DateTime CreateAt { get; set; }

        [Column("update_at")]
        public DateTime UpdateAt { get; set; }

        [Column("quyen")]
        public int Quyen { get; set; }

        [Column("status")]
        public int Status { get; set; }
    }
}