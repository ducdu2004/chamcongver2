using chamcong.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace chamcong.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendancesController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        public class CheckOutRequest
        {
            public int EmployeeId { get; set; }
        }

        [HttpPost("check-out")]
        [Authorize]
        public async Task<IActionResult> CheckOut([FromBody] CheckOutRequest req)
        {
            var result = await _attendanceService.CheckOutAsync(req.EmployeeId);
            return result.Success ? Ok(result.Message) : StatusCode(result.StatusCode, result.Message);
        }
    }
}
