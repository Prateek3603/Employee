using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeManagement.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly string _connectionString;

        public AttendanceController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection() => new SqlConnection(_connectionString);

        // LIST
        public IActionResult Index()
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

            return View(list);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            BindEmployees();
            return View(new AttendanceRecord
            {
                AttendanceDate = DateTime.Today
            });
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AttendanceRecord model)
        {
            if (!ModelState.IsValid)
            {
                BindEmployees();
                return View(model);
            }

            using var conn = GetConnection();
            using var cmd = new SqlCommand("spAttendance_Upsert", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeId", model.EmployeeId);
            cmd.Parameters.AddWithValue("@AttendanceDate", model.AttendanceDate.Date);
            cmd.Parameters.AddWithValue("@Status", model.Status);
            cmd.Parameters.AddWithValue("@Remarks", (object?)model.Remarks ?? DBNull.Value);

            conn.Open();
            cmd.ExecuteNonQuery();

            return RedirectToAction(nameof(Index));
        }

        // EDIT (GET)
        public IActionResult Edit(int id)
        {
            var record = GetAttendanceById(id);
            if (record == null)
                return NotFound();

            BindEmployees();
            return View(record);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, AttendanceRecord model)
        {
            if (id != model.AttendanceId)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                BindEmployees();
                return View(model);
            }

            // Update existing attendance
            using var conn = GetConnection();
            using var cmd = new SqlCommand("spAttendance_Upsert", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeId", model.EmployeeId);
            cmd.Parameters.AddWithValue("@AttendanceDate", model.AttendanceDate.Date);
            cmd.Parameters.AddWithValue("@Status", model.Status);
            cmd.Parameters.AddWithValue("@Remarks", (object?)model.Remarks ?? DBNull.Value);

            conn.Open();
            cmd.ExecuteNonQuery();

            return RedirectToAction(nameof(Index));
        }

        // DELETE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("spAttendance_Delete", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AttendanceId", id);

            conn.Open();
            cmd.ExecuteNonQuery();

            return RedirectToAction(nameof(Index));
        }

        // ---------- helpers ----------

        private AttendanceRecord? GetAttendanceById(int id)
        {
            AttendanceRecord? record = null;

            using var conn = GetConnection();
            using var cmd = new SqlCommand("spAttendance_GetById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AttendanceId", id);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                record = new AttendanceRecord
                {
                    AttendanceId = (int)reader["AttendanceId"],
                    EmployeeId = (int)reader["EmployeeId"],
                    EmployeeName = reader["EmployeeName"].ToString(),
                    AttendanceDate = (DateTime)reader["AttendanceDate"],
                    Status = reader["Status"].ToString(),
                    Remarks = reader["Remarks"] as string
                };
            }

            return record;
        }

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
                    Name = reader["Name"].ToString()
                });
            }

            ViewBag.Employees = new SelectList(employees, "EmployeeId", "Name");
        }
    }
}
