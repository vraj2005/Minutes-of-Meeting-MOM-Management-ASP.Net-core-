using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MOM_Project.Models;
using System.Data;

namespace MOM_Project.Controllers
{
    public class MeetingMemberController : Controller
    {
        private readonly string _connectionString;

        public MeetingMemberController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MOM_DB")
                ?? throw new InvalidOperationException("Connection string 'MOM_DB' not found.");
        }

        public IActionResult Index()
        {
            var members = new List<MeetingMember>();
            var meetings = LoadMeetings();
            var staff = LoadStaff();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("dbo.PR_MOM_MeetingMember_SelectAll", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    members.Add(new MeetingMember
                    {
                        MeetingMemberID = reader.GetInt32(reader.GetOrdinal("MeetingMemberID")),
                        MeetingID = GetMeetingIdByDate(meetings, reader.GetDateTime(reader.GetOrdinal("MeetingDate"))),
                        StaffID = GetStaffIdByName(staff, reader.GetString(reader.GetOrdinal("StaffName"))),
                        IsPresent = reader.GetBoolean(reader.GetOrdinal("IsPresent")),
                        Remarks = reader.IsDBNull(reader.GetOrdinal("Remarks")) ? null : reader.GetString(reader.GetOrdinal("Remarks"))
                    });
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Failed to load attendance records.");
            }

            ViewBag.Meetings = meetings;
            ViewBag.Staff = staff;
            return View(members);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MeetingMember member)
        {
            if (member.MeetingID <= 0 || member.StaffID <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("dbo.PR_MOM_MeetingMember_Insert", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@MeetingID", member.MeetingID);
                command.Parameters.AddWithValue("@StaffID", member.StaffID);
                command.Parameters.AddWithValue("@IsPresent", member.IsPresent);
                command.Parameters.AddWithValue("@Remarks", (object?)member.Remarks ?? DBNull.Value);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Failed to add member to meeting.");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MeetingMember member)
        {
            if (member.MeetingMemberID <= 0 || member.MeetingID <= 0 || member.StaffID <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("dbo.PR_MOM_MeetingMember_UpdateByPK", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@MeetingMemberID", member.MeetingMemberID);
                command.Parameters.AddWithValue("@MeetingID", member.MeetingID);
                command.Parameters.AddWithValue("@StaffID", member.StaffID);
                command.Parameters.AddWithValue("@IsPresent", member.IsPresent);
                command.Parameters.AddWithValue("@Remarks", (object?)member.Remarks ?? DBNull.Value);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Failed to update attendance record.");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int meetingMemberId)
        {
            if (meetingMemberId <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("dbo.PR_MOM_MeetingMember_DeleteByPK", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@MeetingMemberID", meetingMemberId);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException ex) when (ex.Number is 547)
            {
                TempData["ErrorMessage"] = "Cannot remove this attendance record because it is referenced by other records. Remove dependent records first.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Failed to remove attendance record.";
            }

            return RedirectToAction(nameof(Index));
        }

        private List<Meeting> LoadMeetings()
        {
            var meetings = new List<Meeting>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("dbo.PR_MOM_Meetings_SelectAll", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                meetings.Add(new Meeting
                {
                    MeetingID = reader.GetInt32(reader.GetOrdinal("MeetingID")),
                    MeetingDate = reader.GetDateTime(reader.GetOrdinal("MeetingDate"))
                });
            }

            return meetings;
        }

        private List<Staff> LoadStaff()
        {
            var staffMembers = new List<Staff>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("dbo.PR_MOM_Staff_SelectAll", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                staffMembers.Add(new Staff
                {
                    StaffID = reader.GetInt32(reader.GetOrdinal("StaffID")),
                    StaffName = reader.GetString(reader.GetOrdinal("StaffName")),
                    DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName"))
                });
            }

            return staffMembers;
        }

        private static int GetStaffIdByName(IEnumerable<Staff> staff, string staffName)
            => staff.FirstOrDefault(s => s.StaffName == staffName)?.StaffID ?? 0;

        private static int GetMeetingIdByDate(IEnumerable<Meeting> meetings, DateTime meetingDate)
            => meetings.FirstOrDefault(m => m.MeetingDate == meetingDate)?.MeetingID ?? 0;
    }
}
