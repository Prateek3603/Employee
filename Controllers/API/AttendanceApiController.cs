using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System;
using EmployeeManagement.Models;

namespace EmployeeManagement.Controllers.Api
{
    [Route("api/attendance")]
    [ApiController]
    public class AttendanceApiController : ControllerBase
    {
        private readonly string _connectionString;

        public AttendanceApiController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection() => new SqlConnection(_connectionString);

        // GET: api/attendance
        [HttpGet]
        public ActionResult<IEnumerable<AttendanceRecord>> GetAll()
        {
            var list = new List<AttendanceRecord>();

            using var conn = GetConnection();
            using var cmd = new SqlCommand("spAttendance_GetAll", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new AttendanceRecord
                {
                    AttendanceId = (int)reader["AttendanceId"],
                    EmployeeId = (int)reader["EmployeeId"],
                    EmployeeName = reader["EmployeeName"].ToString(),
                    AttendanceDate = (DateTime)reader["AttendanceDate"],
                    Status = reader["Status"].ToString(),
                    Remarks = reader["Remarks"] as string
                });
            }

            return Ok(list);
        }

        // GET: api/attendance/by-date?date=2025-01-01
        [HttpGet("by-date")]
        public ActionResult<IEnumerable<AttendanceRecord>> GetByDate([FromQuery] DateTime date)
        {
            var list = new List<AttendanceRecord>();

            using var conn = GetConnection();
            using var cmd = new SqlCommand("spAttendance_GetByDate", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AttendanceDate", date.Date);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new AttendanceRecord
                {
                    AttendanceId = (int)reader["AttendanceId"],
                    EmployeeId = (int)reader["EmployeeId"],
                    EmployeeName = reader["EmployeeName"].ToString(),
                    AttendanceDate = (DateTime)reader["AttendanceDate"],
                    Status = reader["Status"].ToString(),
                    Remarks = reader["Remarks"] as string
                });
            }

            return Ok(list);
        }

        // POST: api/attendance  (Upsert)
        [HttpPost]
        public IActionResult Upsert(AttendanceRecord model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using var conn = GetConnection();
            using var cmd = new SqlCommand("spAttendance_Upsert", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeId", model.EmployeeId);
            cmd.Parameters.AddWithValue("@AttendanceDate", model.AttendanceDate.Date);
            cmd.Parameters.AddWithValue("@Status", model.Status);
            cmd.Parameters.AddWithValue("@Remarks", (object?)model.Remarks ?? DBNull.Value);

            conn.Open();
            cmd.ExecuteNonQuery();

            return Ok();
        }
    }
}
