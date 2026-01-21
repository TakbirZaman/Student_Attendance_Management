using AutoMapper;
using BLL;
using BLL.DTOs;
using BLL.Services;
using DAL.EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace APIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        StudentService service;

        public StudentController(StudentService service)
        {
            this.service = service;
        }

        [HttpGet("all")]

        public IActionResult All()
        {
            var data = service.Get();
            return Ok(data);
        }

        [HttpGet("{id}")]   

        public IActionResult Get(int id)
        {
            var data = service.Get(id);   
            return Ok(data);
        }


        [HttpPost("create")]
        public IActionResult Create(StudentDTO s)
        {

            var res = service.Create(s); //request go on service 
            if (res == true)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }

        [HttpPost("update")]
        public IActionResult Update(StudentDTO s)
        {

            var res = service.Update(s);  
            if (res == true)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }

        [HttpPost("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var res = service.Delete(id);
            if (res == true)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }

        [HttpGet("{id}/attendances")]
        public IActionResult GetWithAttendance(int id)
        {
            var res = service.GetWithAttendance(id);
            return Ok(res);
        }


        [HttpGet("name/{name}")]
        public StudentDTO FindByName(string name)
        {
               var res = service.FindByName(name);
            return res;
        }

        [HttpGet("name/{name}/attendances")]    
        public StudentAttendanceDTO FindByNameWithAttendances(string name)
        {
            var res = service.FindByNameWithAttendances(name);
            return res;
        }





        [HttpGet("highestattendance")]
        public StudentAttendanceDTO HighestAttendance() { 
            var res = service.HighestAttendance();
            return res;
            }

      

        [HttpGet("top3attendance")]
        public List<StudentAttendanceDTO> Top3Attendance()
        {
            var res = service.Top3Attendance();
            return res;
        }

        [HttpGet("Low3Attendance")]
        public List<StudentAttendanceDTO> Low3Attendance()
        {
            var res = service.Low3Attendance();
            return res;
        }   

        [HttpGet("lowestattendance")]
        public StudentAttendanceDTO LowestAttendance()
        {
            var res = service.HighestAttendance();
            return res;
        }
    }
}
