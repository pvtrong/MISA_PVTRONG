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
    public class PositionsController : ControllerBase
    {
        string connectionString = "User Id=nvmanh;Host=103.124.92.43;port=3306;password=12345678;Database=MS2_31_PVTRONG_CukCuk;Character Set=utf8";
        IDbConnection dbConnection;
        public PositionsController()
        {
                dbConnection = new MySqlConnection(connectionString);
        }
        [HttpGet()]
        public IActionResult Get()
        {
            List<Position> positions = new List<Position>();

            // Lấy dữ liệu:
            positions = dbConnection.Query<Position>("Proc_GetAllPositions", commandType: CommandType.StoredProcedure).ToList();

            return Ok(positions);
        }

        // GET api/<PositionsController>/5
        [HttpGet("{positionId}")]
        public IActionResult Get(Guid positionId)
        {
            var positoin = new Position();

            var sqlQuery = $"Proc_PositionById";
            // Lấy dữ liệu:
            positoin = dbConnection.Query<Position>(sqlQuery, new { PositionId = positionId.ToString() }, commandType: CommandType.StoredProcedure).FirstOrDefault();
           
            return Ok(positoin);
        }

        // GET api/<PositionsController>/search
        [HttpGet("search")]
        public IActionResult Get([FromQuery] string keyword)
        {
            var positoins = new List<Position>();

            // Lấy dữ liệu:
            positoins = dbConnection.Query<Position>($"Proc_Search", new { keyword = keyword.ToString() }, commandType: CommandType.StoredProcedure).ToList();

            return Ok(positoins);
        }

        // POST api/<PositionsController>
        [HttpPost]
        public IActionResult Post([FromBody] Position position)
        {


            var param = new
            {
                PositionId = "",
                PositionName = position.positionName,
                
            };
            // Lấy dữ liệu:
            var affectedRows = dbConnection.Execute($"Proc_InsertEmployee", param, commandType: CommandType.StoredProcedure);
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
