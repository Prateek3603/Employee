using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System;
using EmployeeManagement.Models;

namespace EmployeeManagement.Controllers.Api
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksApiController : ControllerBase
    {
        private readonly string _connectionString;

        public TasksApiController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection() => new SqlConnection(_connectionString);

        // GET: api/tasks
        [HttpGet]
        public ActionResult<IEnumerable<TaskItem>> GetAll()
        {
            var list = new List<TaskItem>();

            using var conn = GetConnection();
            using var cmd = new SqlCommand("spTasks_GetAll", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new TaskItem
                {
                    TaskId = (int)reader["TaskId"],
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"] as string,
                    EmployeeId = (int)reader["EmployeeId"],
                    EmployeeName = reader["EmployeeName"].ToString(),
                    DueDate = reader["DueDate"] == DBNull.Value ? null : (DateTime?)reader["DueDate"],
                    Status = reader["Status"].ToString()
                });
            }

            return Ok(list);
        }

        // GET: api/tasks/5
        [HttpGet("{id:int}")]
        public ActionResult<TaskItem> Get(int id)
        {
            TaskItem task = null;

            using var conn = GetConnection();
            using var cmd = new SqlCommand("spTasks_GetById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TaskId", id);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                task = new TaskItem
                {
                    TaskId = (int)reader["TaskId"],
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"] as string,
                    EmployeeId = (int)reader["EmployeeId"],
                    DueDate = reader["DueDate"] == DBNull.Value ? null : (DateTime?)reader["DueDate"],
                    Status = reader["Status"].ToString()
                };
            }

            if (task == null) return NotFound();
            return Ok(task);
        }

        // POST: api/tasks
        [HttpPost]
        public ActionResult<TaskItem> Post(TaskItem model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using var conn = GetConnection();
            using var cmd = new SqlCommand("spTasks_Insert", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Title", model.Title);
            cmd.Parameters.AddWithValue("@Description", (object?)model.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EmployeeId", model.EmployeeId);
            cmd.Parameters.AddWithValue("@DueDate", (object?)model.DueDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", model.Status);

            conn.Open();
            var idObj = cmd.ExecuteScalar();
            model.TaskId = Convert.ToInt32(idObj);

            return CreatedAtAction(nameof(Get), new { id = model.TaskId }, model);
        }

        // PUT: api/tasks/5
        [HttpPut("{id:int}")]
        public IActionResult Put(int id, TaskItem model)
        {
            if (id != model.TaskId)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using var conn = GetConnection();
            using var cmd = new SqlCommand("spTasks_Update", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TaskId", model.TaskId);
            cmd.Parameters.AddWithValue("@Title", model.Title);
            cmd.Parameters.AddWithValue("@Description", (object?)model.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EmployeeId", model.EmployeeId);
            cmd.Parameters.AddWithValue("@DueDate", (object?)model.DueDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", model.Status);

            conn.Open();
            cmd.ExecuteNonQuery();

            return NoContent();
        }

        // DELETE: api/tasks/5  (SOFT DELETE)
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("spTasks_SoftDelete", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TaskId", id);

            conn.Open();
            cmd.ExecuteNonQuery();

            return NoContent();
        }
    }
}
