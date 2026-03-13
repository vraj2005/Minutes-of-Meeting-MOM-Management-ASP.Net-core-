using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MOM_Project.Models;
using System.Data;
using MOM_Project.Infrastructure;

namespace MOM_Project.Controllers
{
    [CheckAccess]
    public class HomeController : Controller
    {
		private readonly string _connectionString;

		public HomeController(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("MOM_DB")
				?? throw new InvalidOperationException("Connection string 'MOM_DB' not found.");
		}

		public IActionResult Index()
        {
			var model = new DashboardViewModel();

			try
			{
				using var connection = new SqlConnection(_connectionString);
				connection.Open();

				model.TotalDepartments = ExecuteCount(connection, "dbo.MOM_Department");
				model.TotalStaff = ExecuteCount(connection, "dbo.MOM_Staff");

				var startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
				var startOfNextMonth = startOfMonth.AddMonths(1);
				model.TotalMeetingsThisMonth = ExecuteMeetingCount(connection, startOfMonth, startOfNextMonth, cancelledOnly: false);
				model.TotalMeetingsCancelledThisMonth = ExecuteMeetingCount(connection, startOfMonth, startOfNextMonth, cancelledOnly: true);

				var departments = LoadDepartments(connection);
				var venues = LoadVenues(connection);
				model.UpcomingMeetings = LoadUpcomingMeetings(connection, departments, venues);

				BuildLastSixMonthsChart(connection, model);
			}
			catch
			{
				// Dashboard should still render even if DB is unavailable.
			}

			return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

		private static int ExecuteCount(SqlConnection connection, string tableName)
		{
			using var command = new SqlCommand($"SELECT COUNT(1) FROM {tableName}", connection);
			return Convert.ToInt32(command.ExecuteScalar());
		}

		private static int ExecuteMeetingCount(SqlConnection connection, DateTime start, DateTime end, bool cancelledOnly)
		{
			using var command = new SqlCommand(@"SELECT COUNT(1)
FROM dbo.MOM_Meetings
WHERE MeetingDate >= @StartDate AND MeetingDate < @EndDate
  AND (@CancelledOnly = 0 OR IsCancelled = 1)", connection);

			command.Parameters.AddWithValue("@StartDate", start);
			command.Parameters.AddWithValue("@EndDate", end);
			command.Parameters.AddWithValue("@CancelledOnly", cancelledOnly);
			return Convert.ToInt32(command.ExecuteScalar());
		}

		private static List<Department> LoadDepartments(SqlConnection connection)
		{
			var departments = new List<Department>();

			using var command = new SqlCommand("dbo.PR_MOM_Department_SelectAll", connection)
			{
				CommandType = CommandType.StoredProcedure
			};

			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				departments.Add(new Department
				{
					DepartmentID = reader.GetInt32(reader.GetOrdinal("DepartmentID")),
					DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName"))
				});
			}

			reader.Close();
			return departments;
		}

		private static List<MeetingVenue> LoadVenues(SqlConnection connection)
		{
			var venues = new List<MeetingVenue>();

			using var command = new SqlCommand("dbo.PR_MOM_MeetingVenue_SelectAll", connection)
			{
				CommandType = CommandType.StoredProcedure
			};

			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				venues.Add(new MeetingVenue
				{
					MeetingVenueID = reader.GetInt32(reader.GetOrdinal("MeetingVenueID")),
					MeetingVenueName = reader.GetString(reader.GetOrdinal("MeetingVenueName"))
				});
			}

			reader.Close();
			return venues;
		}

		private static List<UpcomingMeetingRow> LoadUpcomingMeetings(SqlConnection connection, List<Department> departments, List<MeetingVenue> venues)
		{
			var list = new List<UpcomingMeetingRow>();
			using var command = new SqlCommand(@"SELECT TOP (5)
	MeetingID,
	MeetingDescription,
	MeetingDate,
	DepartmentID,
	MeetingVenueID,
	ISNULL(IsCancelled, 0) AS IsCancelled
FROM dbo.MOM_Meetings
WHERE MeetingDate >= GETDATE() AND MeetingDate < DATEADD(day, 7, GETDATE())
ORDER BY MeetingDate ASC", connection);

			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				var departmentId = reader.GetInt32(reader.GetOrdinal("DepartmentID"));
				var venueId = reader.GetInt32(reader.GetOrdinal("MeetingVenueID"));

				list.Add(new UpcomingMeetingRow
				{
					MeetingID = reader.GetInt32(reader.GetOrdinal("MeetingID")),
					MeetingDescription = reader.IsDBNull(reader.GetOrdinal("MeetingDescription")) ? string.Empty : reader.GetString(reader.GetOrdinal("MeetingDescription")),
					MeetingDate = reader.GetDateTime(reader.GetOrdinal("MeetingDate")),
					DepartmentName = departments.FirstOrDefault(d => d.DepartmentID == departmentId)?.DepartmentName,
					VenueName = venues.FirstOrDefault(v => v.MeetingVenueID == venueId)?.MeetingVenueName,
					IsCancelled = reader.GetBoolean(reader.GetOrdinal("IsCancelled"))
				});
			}

			reader.Close();
			return list;
		}

		private static void BuildLastSixMonthsChart(SqlConnection connection, DashboardViewModel model)
		{
			var now = DateTime.Today;
			var startMonth = new DateTime(now.Year, now.Month, 1).AddMonths(-5);

			for (var i = 0; i < 6; i++)
			{
				var start = startMonth.AddMonths(i);
				var end = start.AddMonths(1);
				model.ChartCategories.Add(start.ToString("MMM"));
				model.ChartMeetingsHeld.Add(ExecuteMeetingCount(connection, start, end, cancelledOnly: false));
				model.ChartMeetingsCancelled.Add(ExecuteMeetingCount(connection, start, end, cancelledOnly: true));
			}
		}
    }
}
