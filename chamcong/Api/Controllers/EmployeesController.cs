using chamcong.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace chamcong.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetEmployees()
        {
            var result = await _employeeService.GetEmployeesSummaryAsync();
            return result.Success ? Ok(result.Data) : StatusCode(result.StatusCode, result.Message);
        }

        [HttpGet("daily-workers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDailyWorkers()
        {
            var result = await _employeeService.GetDailyWorkersAsync();
            return result.Success ? Ok(result.Data) : StatusCode(result.StatusCode, result.Message);
        }

        [HttpGet("{id}/attendance-sheet")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAttendanceSheet(int id, [FromQuery] int month, [FromQuery] int year)
        {
            var result = await _employeeService.GetAttendanceSheetAsync(id, month, year);
            return result.Success ? Ok(result.Data) : StatusCode(result.StatusCode, result.Message);
        }
    }
}
