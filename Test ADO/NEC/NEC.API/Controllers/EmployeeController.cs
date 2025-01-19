using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NEC.API.DAL;
using NEC.API.Models;

namespace NEC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeDAL _employeeDAL;
        private readonly ILogger<WeatherForecastController> _logger;

        public EmployeeController(EmployeeDAL employeeDAL, ILogger<WeatherForecastController> logger)
        {
            _employeeDAL = employeeDAL;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("api/Employee/GetAll Executed");
            var employee = _employeeDAL.GetAll();
            return Ok(employee);
            /**
             * var a = new List<Employee>
           {
            new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1980, 1, 15),
                Email = "john.doe@example.com",
                Salary = 50000
            },
            new Employee
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 5, 20),
                Email = "jane.smith@example.com",
                Salary = 60000
            },
            // Add more sample employees as needed
           };
            return Ok(a);
            */
        }

        [HttpPost("CreateEmployee")]
        public IActionResult Create(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model Data Is Invalid");
            }
            var result = _employeeDAL.Insert(employee);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id) 
        {
            var emp = _employeeDAL.GetById(id); 
            return Ok(emp);
        }
        
        [HttpPut("Update")]
        public IActionResult Update(int id, Employee employee)
        {
            var updated = _employeeDAL.Update(id, employee);

            return Ok(updated);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var deleted = _employeeDAL.Delete(id);
            return Ok($"deleted {deleted}");
        }
    }
}
