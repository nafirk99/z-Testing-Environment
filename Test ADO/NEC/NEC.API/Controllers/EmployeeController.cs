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

        public EmployeeController(EmployeeDAL employeeDAL)
        {
            _employeeDAL = employeeDAL;
        }

        [HttpGet("GetAll")]
        public  IActionResult GetAll()
        {
            var employee = _employeeDAL.GetAll();
            return Ok(employee);



           // var a =  new List<Employee>
           //{
           // new Employee
           // {
           //     Id = 1,
           //     FirstName = "John",
           //     LastName = "Doe",
           //     DateOfBirth = new DateTime(1980, 1, 15),
           //     Email = "john.doe@example.com",
           //     Salary = 50000
           // },
           // new Employee
           // {
           //     Id = 2,
           //     FirstName = "Jane",
           //     LastName = "Smith",
           //     DateOfBirth = new DateTime(1985, 5, 20),
           //     Email = "jane.smith@example.com",
           //     Salary = 60000
           // },
           // // Add more sample employees as needed
           //};
           // return Ok(a);

        }
    }
}
