using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chamcong.Domain.Entities
{
    public class AttendanceLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public DateTime WorkDate { get; set; }

        public DateTime? CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OvertimeHours { get; set; }

        // Navigation properties
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}
