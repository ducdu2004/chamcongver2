using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chamcong.Domain.Entities
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int WorkshopId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        // 1: Giờ, 2: Sản phẩm
        public int PayType { get; set; }

        [StringLength(50)]
        public string? FingerprintId { get; set; }

        // Navigation properties
        [ForeignKey("WorkshopId")]
        public Workshop Workshop { get; set; }

        public ICollection<ProductionLog> ProductionLogs { get; set; } = new List<ProductionLog>();
        public ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>();
    }
}
