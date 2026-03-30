using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttendanceController : ControllerBase
    {
        AttendanceService service;

        public AttendanceController(AttendanceService service)
        {
            this.service = service;
        }

        [HttpGet("all")]
        public IActionResult All(int pageNumber = 1, int pageSize = 10, int? studentId = null, string status = null)
        {
            var data = service.Get();

            // Filter by student ID
            if (studentId.HasValue)
                data = data.Where(a => a.SId == studentId.Value).ToList();

            // Filter by status
            if (!string.IsNullOrEmpty(status))
                data = data.Where(a => a.Status == status).ToList();

            // Pagination
            var totalCount = data.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var paginatedData = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var response = new PaginatedResponse<AttendanceDTO>
            {
                Data = paginatedData,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasNextPage = pageNumber < totalPages,
                HasPreviousPage = pageNumber > 1
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var data = service.Get(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        [HttpPost("create")]
        public IActionResult Create(AttendanceDTO a)
        {
            var res = service.Create(a);
            if (res == true)
            {
                return Ok(new { success = true, message = "Attendance recorded successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to create attendance" });
            }
        }

        [HttpPost("update")]
        public IActionResult Update(AttendanceDTO a)
        {
            var res = service.Update(a);
            if (res == true)
            {
                return Ok(new { success = true, message = "Attendance updated successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to update attendance" });
            }
        }

        [HttpPost("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var res = service.Delete(id);
            if (res == true)
            {
                return Ok(new { success = true, message = "Attendance deleted successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "Failed to delete attendance" });
            }
        }
    }
}
