using chamcong.Application.DTOs;
using chamcong.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace chamcong.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssueReportsController : ControllerBase
    {
        private readonly IIssueReportService _issueReportService;

        public IssueReportsController(IIssueReportService issueReportService)
        {
            _issueReportService = issueReportService;
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> CreateReport([FromBody] CreateIssueReportDto dto)
        {
            var empIdClaim = User.FindFirst("EmployeeId");
            if (empIdClaim == null) return Unauthorized("Không tìm thấy EmployeeId.");

            var employeeId = int.Parse(empIdClaim.Value);
            var result = await _issueReportService.CreateReportAsync(employeeId, dto);
            
            if (!result.Success) return StatusCode(result.StatusCode, result.Message);
            return StatusCode(201, result.Data);
        }

        [HttpGet("pending")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPendingReports()
        {
            var result = await _issueReportService.GetPendingReportsAsync();
            if (!result.Success) return StatusCode(result.StatusCode, result.Message);
            return Ok(result.Data);
        }

        [HttpPut("{id}/resolve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResolveReport(int id)
        {
            var result = await _issueReportService.ResolveReportAsync(id);
            if (!result.Success) return StatusCode(result.StatusCode, result.Message);
            return Ok(new { Message = "Đã xác nhận giải quyết báo cáo." });
        }
    }
}
