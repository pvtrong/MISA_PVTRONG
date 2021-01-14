using Dapper;
using Microsoft.AspNetCore.Mvc;
using MISA.DEMO.API.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.DEMO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {


        string connectionString = "User Id=nvmanh;Host=103.124.92.43;port=3306;password=12345678;Database=MS2_31_PVTRONG_CukCuk;Character Set=utf8";
        IDbConnection dbConnection;

        public EmployeesController()
        {
             dbConnection = new MySqlConnection(connectionString);
        }

        /// <summary>
        ///PVTRONG 31/12/2020
        /// </summary>
        /// <param name="limitParam">Số lượng bản ghi được lấy</param>
        /// <param name="offsetParam">Bắt đầu từ bản ghi nào</param>
        /// <returns>Danh sách bản ghi nhân viên</returns>
        [HttpGet("")]
        public IActionResult Get([FromQuery] string limitParam, [FromQuery] string offsetParam)
        {
            List<Employee> employees = new List<Employee>();
            
            // Lấy dữ liệu:
            employees = dbConnection.Query<Employee>("Proc_GetAllEmployeesWithPaging", new {LimitParam = limitParam, OffsetParam = offsetParam }, commandType: CommandType.StoredProcedure).ToList();
            
            return Ok(employees);
            }

        /// <summary>
        /// PVTRONG 31/12/2020
        /// </summary>
        /// <returns>Trả về mã số nhân viên lớn nhất </returns>
        [HttpGet("EmployeeCodeMax")]
        public IActionResult Get()
        {
            var employee = new Employee();
            // Lấy dữ liệu:
            var employeeCode = dbConnection.Query<Employee>("Proc_GetEmployeeCodeMax", commandType: CommandType.StoredProcedure).FirstOrDefault();

            return Ok(employeeCode);
        }

        /// <summary>
        /// PVTRONG 31/12/2020
        /// </summary>
        /// <param name="employeeId">ID của nhân viên</param>
        /// <returns>Trả về thông tin của nhân viên đó</returns>
        // GET api/<EmployeesController>/5
        [HttpGet("{employeeId}")]
        public IActionResult Get(Guid employeeId)
        {
            var employee = new Employee();
            
            var sqlQuery = $"Proc_GetEmployeeById";
            // Lấy dữ liệu:
            employee = dbConnection.Query<Employee>(sqlQuery, new { EmployeeId = employeeId.ToString()}, commandType:CommandType.StoredProcedure).FirstOrDefault();
            
            return Ok(employee);
        }

        /// <summary>
        /// PVTRONG 31/12/2020
        /// </summary>
        /// <param name="keyword">từ khóa tìm kiếm</param>
        /// <param name="positionId">Vị trí/ chức vụ</param>
        /// <param name="departmentId">Phòng ban nào</param>
        /// <param name="limitParam">Số lượng bản ghi lấy</param>
        /// <param name="offsetParam">Lấy từ bản ghi thứ bao nhiêu</param>
        /// <returns>Trả về danh sách nhân viên theo tìm kiếm</returns>
        // GET api/<EmployeesController>/search
        [HttpGet("search")]
        public IActionResult Get([FromQuery] string keyword, [FromQuery] string positionId, [FromQuery] string departmentId,[FromQuery] string limitParam,[FromQuery] string offsetParam)
        {
            var employees = new List<Employee>();
            if (keyword == null)
            {
                keyword = "N";
            }
            if (positionId == null)
            {
                positionId = "-";
            }
            else positionId = positionId.ToString(); 
            if (departmentId == null)
            {
                departmentId = "-";
            }
            else departmentId = departmentId.ToString();
            // Lấy dữ liệu:
            employees = dbConnection.Query<Employee>($"Proc_Search", new { keyword = keyword.ToString(), positiont = positionId, departmentt = departmentId, LimitParam = limitParam, OffsetParam = offsetParam }, commandType: CommandType.StoredProcedure).ToList();
           
            return Ok(employees);
        }

        /// <summary>
        /// PVTRONG 31/12/2020
        /// </summary>
        /// <param name="EmployeeCode">Mã nhân viên</param>
        /// <returns>Trả về xem mã nhân viên có trong danh sách chưa 1: rồi, 0: chưa</returns>
        [HttpGet("CheckEmployeeCode")]
        public IActionResult Get([FromQuery] string EmployeeCode)
        {
            var employees = new Employee();
            if (EmployeeCode == null) return NoContent();
            
            // Lấy dữ liệu:
            employees = dbConnection.Query<Employee>($"Proc_CheckEmployeeCode", new { employeeCodeParam = EmployeeCode.ToString() }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            return Ok(employees);
        }

        /// <summary>
        /// PVTRONG 31/12/2020
        /// </summary>
        /// <param name="employee">nhân viên được thêm</param>
        /// <returns>Trả về tình trạng thêm</returns>
        // POST api/<EmployeesController>
        [HttpPost]
        public IActionResult Post([FromBody] Employee employee)
        {

            var param = new
            {
                EmployeeId = "",
                EmployeeCode = employee.employeeCode,
                FullName = employee.fullName,
                DateOfBirth = employee.dateOfBirth,
                Gender = employee.gender,
                IdentifyNumber = employee.identifyNumber,
                DateOfIN = employee.dateOfIN,
                PlaceOfIN = employee.placeOfIN,
                Email = employee.email,
                PhoneNumber = employee.phoneNumber,
                PositionId = employee.positionId.ToString(),
                DepartmentId = employee.departmentId.ToString(),
                TaxCode = employee.taxCode,
                Salary = employee.salary,
                DateOfBeginWork = employee.dateOfBeginWork,
                Status = employee.status
            };
            // Lấy dữ liệu:
            var affectedRows = dbConnection.Execute($"Proc_InsertEmployee", param, commandType:CommandType.StoredProcedure);
            if (affectedRows > 0)
            {
                return CreatedAtAction("POST", affectedRows);
            }
             return NoContent();
            
            //Employee.employee.Add(customer);
            
        }

        /// <summary>
        /// PVTRONG 31/12/2020
        /// </summary>
        /// <param name="employee">nhân viên được sửa</param>
        /// <returns>Trả về tình trạng sửa </returns>
        // PUT api/<EmployeesController>/5
        [HttpPut]
        public IActionResult Put( [FromBody] Employee employee)
        {
            var sqlQuery = $"Proc_UpdateEmployee";
            var param = new
            {
                EmployeeId = employee.employeeId.ToString(),
                EmployeeCode = employee.employeeCode,
                FullName = employee.fullName,
                DateOfBirth = employee.dateOfBirth,
                Gender = employee.gender,
                IdentifyNumber = employee.identifyNumber,
                DateOfIN = employee.dateOfIN,
                PlaceOfIN = employee.placeOfIN,
                Email = employee.email,
                PhoneNumber = employee.phoneNumber,
                PositionId = employee.positionId.ToString(),
                DepartmentId = employee.departmentId.ToString(),
                TaxCode = employee.taxCode,
                Salary = employee.salary,
                DateOfBeginWork = employee.dateOfBeginWork,
                Status = employee.status,
                ModifiedDate = DateTime.Now,
                ModifiedBy = ""

            };
            var affectedRows = dbConnection.Execute(sqlQuery, param, commandType: CommandType.StoredProcedure);
            if (affectedRows > 0)
            {
                return CreatedAtAction("POST", affectedRows);
            }
            return NoContent();
        }

        /// <summary>
        /// PVTRONG 31/12/2020
        /// </summary>
        /// <param name="employeeIds">ID của nhân viên được xóa</param>
        /// <returns>Trả về tình trạng xóa</returns>
        // DELETE api/<CustomersController>/5
        [HttpDelete]
        public IActionResult Delete([FromBody] string[] employeeIds)
        {

            var sqlQuery = $"Proc_DeleteEmployee";
            foreach (string item in employeeIds)
            
            {
                dbConnection.Query(sqlQuery, new { EmployeeId = item.ToString() }, commandType: CommandType.StoredProcedure);
            }
            return Ok();
        }
    }
}
