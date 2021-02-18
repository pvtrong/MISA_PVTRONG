using Dapper;
using Microsoft.AspNetCore.Mvc;
using MISA.Common.Models;
using MISA.Service;
using MISA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MISA.Service.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.DEMO.API.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : BaseController<Employee>
    {
        
        public EmployeesController(IEmployeeService _employeeService) : base(_employeeService)
        {

        }


        /// <summary>
        /// PVTRONG 31/12/2020
        /// </summary>
        /// <returns>Trả về mã số nhân viên lớn nhất </returns>
        //[HttpGet("EmployeeCodeMax")]
        //public IActionResult Get()
        //{
        //    var employee = new Employee();
        //    // Lấy dữ liệu:
        //    var employeeCode = dbConnection.Query<Employee>("Proc_GetEmployeeCodeMax", commandType: CommandType.StoredProcedure).FirstOrDefault();

        //    return Ok(employeeCode);
        //}

        ///// <summary>
        ///// PVTRONG 31/12/2020
        ///// </summary>
        ///// <param name="employeeIds">ID của nhân viên được xóa</param>
        ///// <returns>Trả về tình trạng xóa</returns>
        //// DELETE api/<CustomersController>/5
        //[HttpDelete]
        //public IActionResult Delete([FromBody] string[] employeeIds)
        //{

        //    var sqlQuery = $"Proc_DeleteEmployee";
        //    foreach (string item in employeeIds)

        //    {
        //        dbConnection.Query(sqlQuery, new { EmployeeId = item.ToString() }, commandType: CommandType.StoredProcedure);
        //    }
        //    return Ok();
        //}
    }
}
