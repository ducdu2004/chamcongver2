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

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal HourlyWage { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OvertimeHourlyWage { get; set; }

        // Navigation properties
        [ForeignKey("WorkshopId")]
        public Workshop Workshop { get; set; }

        public ICollection<ProductionLog> ProductionLogs { get; set; } = new List<ProductionLog>();
        public ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>();
        public ICollection<EmploymentHistory> EmploymentHistories { get; set; } = new List<EmploymentHistory>();
    }
}
