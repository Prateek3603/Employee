using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly string _connectionString;

        public EmployeesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection() => new SqlConnection(_connectionString);

        public IActionResult Index()
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

            return View(list);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using var conn = GetConnection();
            using var cmd = new SqlCommand("spEmployees_Insert", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", model.Name);
            cmd.Parameters.AddWithValue("@Email", model.Email);
            cmd.Parameters.AddWithValue("@Phone", (object?)model.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Department", (object?)model.Department ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@JoiningDate", model.JoiningDate);

            conn.Open();

            try
            {
               
                cmd.ExecuteScalar();   // 👈 yahi par exception aati thi
            }
            catch (SqlException ex)
            {
                // 2627, 2601 = unique key / duplicate key error
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    ModelState.AddModelError("Email", "This email already exists.");
                    return View(model);
                }

                // koi aur SQL error ho to aage throw kar do
                throw;
            }
                return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var emp = GetById(id);
            if (emp == null) return NotFound();
            return View(emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Employee model)
        {
            if (id != model.EmployeeId) return BadRequest();
            if (!ModelState.IsValid) return View(model);

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

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("spEmployees_SoftDelete", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeId", id);
            conn.Open();
            cmd.ExecuteNonQuery();

            return RedirectToAction(nameof(Index));
        }

        private Employee GetById(int id)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("spEmployees_GetById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeId", id);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Employee
                {
                    EmployeeId = (int)reader["EmployeeId"],
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),
                    Phone = reader["Phone"] as string,
                    Department = reader["Department"] as string,
                    JoiningDate = (DateTime)reader["JoiningDate"]
                };
            }

            return null;
        }
    }
}
