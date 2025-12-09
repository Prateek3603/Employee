using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeManagement.Controllers
{
    public class TasksController : Controller
    {
        private readonly string _connectionString;

        public TasksController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection() => new SqlConnection(_connectionString);

        // ---------------- INDEX --------------------
        public IActionResult Index()
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

            return View(list);
        }

        // ---------------- CREATE GET --------------------
        public IActionResult Create()
        {
            BindEmployees();
            return View();
        }

        // ---------------- CREATE POST --------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaskItem model)
        {
            if (!ModelState.IsValid)
            {
                BindEmployees();
                return View(model);
            }

            using var conn = GetConnection();
            using var cmd = new SqlCommand("spTasks_Insert", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Title", model.Title);
            cmd.Parameters.AddWithValue("@Description", (object?)model.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EmployeeId", model.EmployeeId);
            cmd.Parameters.AddWithValue("@DueDate", (object?)model.DueDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", model.Status);

            conn.Open();
            cmd.ExecuteNonQuery(); // ✔ correct for insert

            return RedirectToAction(nameof(Index));
        }

        // ---------------- EDIT GET --------------------
        public IActionResult Edit(int id)
        {
            var task = GetById(id);
            if (task == null) return NotFound();

            BindEmployees();
            return View(task);
        }

        // ---------------- EDIT POST --------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TaskItem model)
        {
            if (id != model.TaskId) return BadRequest();

            if (!ModelState.IsValid)
            {
                BindEmployees();
                return View(model);
            }

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

            return RedirectToAction(nameof(Index));
        }

        // ---------------- DELETE --------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("spTasks_SoftDelete", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@TaskId", id);

            conn.Open();
            cmd.ExecuteNonQuery();

            return RedirectToAction(nameof(Index));
        }

        // ---------------- GET BY ID --------------------
        private TaskItem GetById(int id)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("spTasks_GetById", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@TaskId", id);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new TaskItem
                {
                    TaskId = (int)reader["TaskId"],
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"] as string,
                    EmployeeId = (int)reader["EmployeeId"],
                    DueDate = reader["DueDate"] == DBNull.Value ? null : (DateTime?)reader["DueDate"],
                    Status = reader["Status"].ToString()
                };
            }

            return null;
        }

        // ---------------- EMPLOYEE DROPDOWN --------------------
        private void BindEmployees()
        {
            var employees = new List<Employee>();

            using var conn = GetConnection();
            using var cmd = new SqlCommand("spEmployees_GetAll", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                employees.Add(new Employee
                {
                    EmployeeId = (int)reader["EmployeeId"],
                    Name = reader["Name"].ToString(),
                    Department = reader["Department"].ToString()
                });
            }

            // 👇 Dropdown me Name + (Department)
            ViewBag.Employees = new SelectList(
                employees.Select(e => new
                {
                    e.EmployeeId,
                    Display = $"{e.Name} ({e.Department})"
                }),
                "EmployeeId",
                "Display"
            );
        }
    }
}
