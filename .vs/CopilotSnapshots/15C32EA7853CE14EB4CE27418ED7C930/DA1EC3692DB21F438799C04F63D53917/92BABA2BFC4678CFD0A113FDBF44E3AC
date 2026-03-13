using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MOM_Project.Models;
using System.Data;

namespace MOM_Project.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly string _connectionString;

        public DepartmentController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MOM_DB")
                ?? throw new InvalidOperationException("Connection string 'MOM_DB' not found.");
        }

        public IActionResult Index()
        {
            var departments = new List<Department>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("PR_MOM_Department_SelectAll", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                connection.Open();
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    departments.Add(new Department
                    {
                        DepartmentID = reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                        DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName")),
                        Remarks = reader.IsDBNull(reader.GetOrdinal("Remarks")) ? null : reader.GetString(reader.GetOrdinal("Remarks")),
                        Created = reader.GetDateTime(reader.GetOrdinal("Created")),
                        Modified = reader.GetDateTime(reader.GetOrdinal("Modified"))
                    });
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Failed to load departments.");
            }

            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department department)
        {
            if (string.IsNullOrWhiteSpace(department.DepartmentName))
            {
                ModelState.AddModelError(nameof(department.DepartmentName), "Department name is required.");
            }

            if (!ModelState.IsValid)
            {
                return View(department);
            }

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("PR_MOM_Department_Insert", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@DepartmentName", department.DepartmentName.Trim());
                command.Parameters.AddWithValue("@Remarks", (object?)department.Remarks ?? DBNull.Value);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Failed to create department.");
                return View(department);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var department = GetDepartmentById(id);
            if (department is null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department department)
        {
            if (department.DepartmentID <= 0 || string.IsNullOrWhiteSpace(department.DepartmentName))
            {
                if (string.IsNullOrWhiteSpace(department.DepartmentName))
                {
                    ModelState.AddModelError(nameof(department.DepartmentName), "Department name is required.");
                }

                return View(department);
            }

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("PR_MOM_Department_UpdateByPK", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@DepartmentID", department.DepartmentID);
                command.Parameters.AddWithValue("@DepartmentName", department.DepartmentName.Trim());
                command.Parameters.AddWithValue("@Remarks", (object?)department.Remarks ?? DBNull.Value);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Failed to update department.");
                return View(department);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var department = GetDepartmentById(id);
            if (department is null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int departmentId)
        {
            if (departmentId <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("PR_MOM_Department_DeleteByPK", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@DepartmentID", departmentId);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException ex) when (ex.Number is 547)
            {
                TempData["ErrorMessage"] = "Cannot delete this department because it is being used in other records (e.g., Staff or Meetings). Remove dependent records first.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Failed to delete department.");
                var department = GetDepartmentById(departmentId);
                if (department is null)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View("Delete", department);
            }

            return RedirectToAction(nameof(Index));
        }

        private Department? GetDepartmentById(int departmentId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("PR_MOM_Department_SelectByPK", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@DepartmentID", departmentId);

                connection.Open();
                using var reader = command.ExecuteReader();

                if (!reader.Read())
                {
                    return null;
                }

                return new Department
                {
                    DepartmentID = reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                    DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName")),
                    Remarks = reader.IsDBNull(reader.GetOrdinal("Remarks")) ? null : reader.GetString(reader.GetOrdinal("Remarks")),
                    Created = reader.GetDateTime(reader.GetOrdinal("Created")),
                    Modified = reader.GetDateTime(reader.GetOrdinal("Modified"))
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
