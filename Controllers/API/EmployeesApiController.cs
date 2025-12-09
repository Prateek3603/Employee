using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System;
using EmployeeManagement.Models;

namespace EmployeeManagement.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesApiController : ControllerBase
    {
        private readonly string _connectionString;

        public EmployeesApiController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection() => new SqlConnection(_connectionString);

        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetAll()
        {
            var list = new List<Employee>();

            using var conn = GetConnection();
            using var cmd = new SqlCommand("spEmployees_GetAll", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Employee
                {
                    EmployeeId = (int)reader["EmployeeId"],
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    Phone = reader["Phone"] as string,
                    Department = reader["Department"] as string,
                    JoiningDate = (DateTime)reader["JoiningDate"]
                });
            }

            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<Employee> Get(int id)
        {
            Employee emp = null;

            using var conn = GetConnection();
            using var cmd = new SqlCommand("spEmployees_GetById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeId", id);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                emp = new Employee
                {
                    EmployeeId = (int)reader["EmployeeId"],
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    Phone = reader["Phone"] as string,
                    Department = reader["Department"] as string,
                    JoiningDate = (DateTime)reader["JoiningDate"]
                };
            }

            if (emp == null) return NotFound();
            return Ok(emp);
        }

        [HttpPost]
        public ActionResult<Employee> Post(Employee model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using var conn = GetConnection();
            using var cmd = new SqlCommand("spEmployees_Insert", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", model.Name);
            cmd.Parameters.AddWithValue("@Email", model.Email);
            cmd.Parameters.AddWithValue("@Phone", (object?)model.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Department", (object?)model.Department ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@JoiningDate", model.JoiningDate);

            conn.Open();
            var idObj = cmd.ExecuteScalar();
            model.EmployeeId = Convert.ToInt32(idObj);

            return CreatedAtAction(nameof(Get), new { id = model.EmployeeId }, model);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Employee model)
        {
            if (id != model.EmployeeId) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            using var conn = GetConnection();
            using var cmd = new SqlCommand("spEmployees_Update", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeId", model.EmployeeId);
            cmd.Parameters.AddWithValue("@Name", model.Name);
            cmd.Parameters.AddWithValue("@Email", model.Email);
            cmd.Parameters.AddWithValue("@Phone", (object?)model.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Department", (object?)model.Department ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@JoiningDate", model.JoiningDate);

            conn.Open();
            cmd.ExecuteNonQuery();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("spEmployees_SoftDelete", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeId", id);

            conn.Open();
            cmd.ExecuteNonQuery();

            return NoContent();
        }
    }
}
