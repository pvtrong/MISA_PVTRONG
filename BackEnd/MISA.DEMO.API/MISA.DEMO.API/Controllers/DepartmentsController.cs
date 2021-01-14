using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.DEMO.API.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.DEMO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : Controller
    {
        
        string connectionString = "User Id=nvmanh;Host=103.124.92.43;port=3306;password=12345678;Database=MS2_31_PVTRONG_CukCuk;Character Set=utf8";
        IDbConnection dbConnection;
        public DepartmentsController()
        {
            dbConnection = new MySqlConnection(connectionString);
        }
        
        [HttpGet()]
        public IActionResult Get()
        {
            List<Department> departments = new List<Department>();

            // Lấy dữ liệu:
            departments = dbConnection.Query<Department>("Proc_GetAllDepartments", commandType: CommandType.StoredProcedure).ToList();

            return Ok(departments);
        }

        // GET api/<DepartmentsController>/5
        [HttpGet("{departmentId}")]
        public IActionResult Get(Guid departmentId)
        {
            var department = new Department();

            var sqlQuery = $"Proc_DepartmentById";
            // Lấy dữ liệu:
            department = dbConnection.Query<Department>(sqlQuery, new { DepartmentId = departmentId.ToString() }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            return Ok(department);
        }

        // GET api/<DepartmentsController>/search
        [HttpGet("search")]
        public IActionResult Get([FromQuery] string keyword)
        {
            var departments = new List<Department>();

            // Lấy dữ liệu:
            departments = dbConnection.Query<Department>($"Proc_Search", new { keyword = keyword.ToString() }, commandType: CommandType.StoredProcedure).ToList();

            return Ok(departments);
        }

        // POST api/<DepartmentsController>
        [HttpPost]
        public IActionResult Post([FromBody] Department department)
        {


            var param = new
            {
                EDepartmentId = "",
                DepartmentName = department.departmentName,

            };
            // Lấy dữ liệu:
            var affectedRows = dbConnection.Execute($"Proc_InsertDepartment", param, commandType: CommandType.StoredProcedure);
            if (affectedRows > 0)
            {
                return CreatedAtAction("POST", affectedRows);
            }
            else return NoContent();

            //Employee.employee.Add(customer);

        }

        // PUT api/<EmployeesController>/5
        [HttpPut("{id}")]
        public void Put(int id)
        {

        }

        // DELETE api/<CustomersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
