using Microsoft.Data.SqlClient;
using MOM_Project.Models;
using System.Data;

namespace MOM_Project.Infrastructure
{
	public static class DatabaseSeeder
	{
		public static void SeedAll(IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("MOM_DB");
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				return;
			}

			try
			{
				using var connection = new SqlConnection(connectionString);
				connection.Open();

				SeedDepartments(connection, targetCount: 25);
				SeedMeetingTypes(connection, targetCount: 25);
				SeedMeetingVenues(connection, targetCount: 25);
				SeedStaff(connection, targetCount: 25);
				SeedMeetings(connection, targetCount: 25);
				SeedMeetingMembers(connection, targetCount: 25);
			}
			catch
			{
				// Intentionally ignore seeding errors.
			}
		}

		public static void SeedDepartmentsOnly(IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("MOM_DB");
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				return;
			}

			try
			{
				using var connection = new SqlConnection(connectionString);
				connection.Open();
				SeedDepartments(connection, targetCount: 12);
			}
			catch
			{
				// Intentionally ignore seeding errors.
			}
		}

		private static void SeedDepartments(SqlConnection connection, int targetCount)
		{
			var existing = LoadDepartments(connection);

			(string Name, string? Remarks)[] seed =
			[
				("HR", "Human Resources"),
				("IT", "Information Technology"),
				("Admin", "Administration"),
				("Finance", "Accounts & Finance"),
				("Sales", "Sales & Business Development"),
				("Marketing", "Branding & Digital"),
				("Operations", "Operations & Logistics"),
				("Support", "Customer Support"),
				("Product", "Product Management"),
				("QA", "Quality Assurance"),
				("Legal", "Legal & Compliance"),
				("R&D", "Research and Development"),
				("Engineering", "Software Engineering"),
				("Design", "UI/UX Design"),
				("Procurement", "Purchasing & Procurement"),
				("Facilities", "Office & Facilities"),
				("Customer Success", "Customer onboarding & success"),
				("Data", "Data & Analytics"),
				("Security", "Information Security"),
				("Compliance", "Process & compliance"),
				("Training", "Learning & development"),
				("Accounts", "Billing & receivables"),
				("Payroll", "Payroll management"),
				("Public Relations", "PR & communications"),
				("Management", "Leadership & management")
			];

			foreach (var item in seed)
			{
				if (existing.Count >= targetCount)
				{
					break;
				}

				if (existing.Any(d => string.Equals(d.DepartmentName, item.Name, StringComparison.OrdinalIgnoreCase)))
				{
					continue;
				}

				using var cmd = new SqlCommand("PR_MOM_Department_Insert", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				cmd.Parameters.AddWithValue("@DepartmentName", item.Name);
				cmd.Parameters.AddWithValue("@Remarks", (object?)item.Remarks ?? DBNull.Value);
				cmd.ExecuteNonQuery();

				existing.Add(new Department { DepartmentName = item.Name });
			}
		}

		private static void SeedMeetingTypes(SqlConnection connection, int targetCount)
		{
			var existing = LoadMeetingTypes(connection);

			(string Name, string? Remarks)[] seed =
			[
				("Weekly Standup", "Team weekly sync"),
				("Daily Sync", "Short daily update"),
				("Sprint Planning", "Plan upcoming sprint"),
				("Sprint Review", "Demo completed work"),
				("Project Review", "Review milestones & blockers"),
				("Client Call", "Client discussion & updates"),
				("Client Onboarding", "New client onboarding"),
				("Training Session", "Internal learning session"),
				("Retrospective", "Sprint retrospective"),
				("One-on-One", "Individual check-in"),
				("Budget Review", "Finance and budget"),
				("Hiring Interview", "Interview round"),
				("Townhall", "Company townhall"),
				("Vendor Discussion", "Vendor / partner meeting"),
				("Security Review", "Security audit & review"),
				("Incident Review", "Incident post-mortem"),
				("Release Planning", "Release readiness"),
				("Architecture Review", "Technical design review"),
				("Design Review", "UI/UX review"),
				("Stakeholder Meeting", "Stakeholder sync"),
				("Performance Review", "Quarterly performance"),
				("Compliance Training", "Mandatory compliance"),
				("Operational Review", "Ops metrics review"),
				("Demo Session", "Feature demo"),
				("Escalation Call", "Urgent escalation"),
				("QBR", "Quarterly business review")
			];

			foreach (var item in seed)
			{
				if (existing.Count >= targetCount)
				{
					break;
				}

				if (existing.Any(t => string.Equals(t.MeetingTypeName, item.Name, StringComparison.OrdinalIgnoreCase)))
				{
					continue;
				}

				using var cmd = new SqlCommand("PR_MOM_MeetingType_Insert", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				cmd.Parameters.AddWithValue("@MeetingTypeName", item.Name);
				cmd.Parameters.AddWithValue("@Remarks", (object?)item.Remarks ?? DBNull.Value);
				cmd.ExecuteNonQuery();

				existing.Add(new MeetingType { MeetingTypeName = item.Name });
			}
		}

		private static void SeedMeetingVenues(SqlConnection connection, int targetCount)
		{
			var existing = LoadMeetingVenues(connection);

			(string Name, string? Remarks)[] seed =
			[
				("Conference Room A", "Main office"),
				("Conference Room B", "Main office"),
				("Conference Room C", "Main office"),
				("Board Room", "Executive meeting room"),
				("Training Room", "Training / workshop"),
				("Meeting Pod 1", "Small discussion room"),
				("Meeting Pod 2", "Small discussion room"),
				("Auditorium", "Large presentations"),
				("Cafeteria", "Informal meeting"),
				("Zoom", "Online"),
				("Google Meet", "Online"),
				("Microsoft Teams", "Online"),
				("Client Site - Ahmedabad", "Onsite"),
				("Client Site - Surat", "Onsite"),
				("Client Site - Vadodara", "Onsite"),
				("Client Site - Rajkot", "Onsite"),
				("Client Site - Gandhinagar", "Onsite"),
				("Remote", "Work from home"),
				("Innovation Lab", "R&D space"),
				("Reception Lounge", "Visitor area"),
				("Floor 2 - Collaboration Area", "Open meeting area"),
				("Floor 3 - Quiet Room", "Low-noise room"),
				("Hotel - Meeting Hall", "External venue"),
				("Co-working Space", "External venue"),
				("Client Office - Pune", "Onsite"),
				("Client Office - Mumbai", "Onsite"),
				("Client Office - Bengaluru", "Onsite")
			];

			foreach (var item in seed)
			{
				if (existing.Count >= targetCount)
				{
					break;
				}

				if (existing.Any(v => string.Equals(v.MeetingVenueName, item.Name, StringComparison.OrdinalIgnoreCase)))
				{
					continue;
				}

				using var cmd = new SqlCommand("PR_MOM_MeetingVenue_Insert", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				cmd.Parameters.AddWithValue("@MeetingVenueName", item.Name);
				cmd.Parameters.AddWithValue("@Remarks", (object?)item.Remarks ?? DBNull.Value);
				cmd.ExecuteNonQuery();

				existing.Add(new MeetingVenue { MeetingVenueName = item.Name });
			}
		}

		private static void SeedStaff(SqlConnection connection, int targetCount)
		{
			var departments = LoadDepartments(connection);
			if (departments.Count == 0)
			{
				return;
			}

			var existing = LoadStaff(connection);

			(string Name, string Mobile, string Email, string? Remarks)[] seed =
			[
				("Vraj Patel", "9876501234", "vraj.patel@example.com", "Team lead"),
				("Tirth Shah", "9898001122", "tirth.shah@example.com", "Backend"),
				("Meet Desai", "9974003344", "meet.desai@example.com", "Frontend"),
				("Harshil Mehta", "9909007788", "harshil.mehta@example.com", "QA"),
				("Krish Patel", "9723504455", "krish.patel@example.com", "Support"),
				("Dhruv Joshi", "9712005566", "dhruv.joshi@example.com", "DevOps"),
				("Yash Soni", "9825006677", "yash.soni@example.com", "Operations"),
				("Neel Trivedi", "9998008899", "neel.trivedi@example.com", "Finance"),
				("Riya Patel", "9811002233", "riya.patel@example.com", "HR"),
				("Aditi Sharma", "9898993344", "aditi.sharma@example.com", "Recruitment"),
				("Jenil Parmar", "9722004455", "jenil.parmar@example.com", "Marketing"),
				("Ishita Shah", "9877005566", "ishita.shah@example.com", "Admin"),
				("Parth Patel", "9824011122", "parth.patel@example.com", "Engineering"),
				("Kunal Mehta", "9726512233", "kunal.mehta@example.com", "Engineering"),
				("Het Patel", "9909013344", "het.patel@example.com", "Design"),
				("Mansi Shah", "9974014455", "mansi.shah@example.com", "Product"),
				("Priya Desai", "9898015566", "priya.desai@example.com", "Customer Success"),
				("Rahul Patel", "9811016677", "rahul.patel@example.com", "Sales"),
				("Siddharth Joshi", "9712017788", "siddharth.joshi@example.com", "Data"),
				("Nisha Mehta", "9722018899", "nisha.mehta@example.com", "Accounts"),
				("Darshan Shah", "9825029900", "darshan.shah@example.com", "Operations"),
				("Ankit Patel", "9876511010", "ankit.patel@example.com", "Support"),
				("Bhavesh Desai", "9898022020", "bhavesh.desai@example.com", "QA"),
				("Pooja Sharma", "9909033030", "pooja.sharma@example.com", "HR"),
				("Nikhil Trivedi", "9974044040", "nikhil.trivedi@example.com", "IT"),
			];

			var deptIndex = 0;
			foreach (var item in seed)
			{
				if (existing.Count >= targetCount)
				{
					break;
				}

				if (existing.Any(s => string.Equals(s.StaffName, item.Name, StringComparison.OrdinalIgnoreCase)))
				{
					continue;
				}

				var deptId = departments[deptIndex % departments.Count].DepartmentID;
				deptIndex++;

				using var cmd = new SqlCommand("dbo.PR_MOM_Staff_Insert", connection)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@DepartmentID", deptId);
				cmd.Parameters.AddWithValue("@StaffName", item.Name);
				cmd.Parameters.AddWithValue("@Mobile", item.Mobile);
				cmd.Parameters.AddWithValue("@Email", item.Email);
				cmd.Parameters.AddWithValue("@Remarks", (object?)item.Remarks ?? DBNull.Value);
				cmd.ExecuteNonQuery();

				existing.Add(new Staff { StaffName = item.Name });
			}
		}

		private static void SeedMeetings(SqlConnection connection, int targetCount)
		{
			var departments = LoadDepartments(connection);
			var types = LoadMeetingTypes(connection);
			var venues = LoadMeetingVenues(connection);

			if (departments.Count == 0 || types.Count == 0 || venues.Count == 0)
			{
				return;
			}

			var existing = LoadMeetingsBasic(connection);

			(string Description, string DocFile, int OffsetDays, int Hour, int Minute)[] seed =
			[
				("MOM Portal - Project Kickoff", "MOM_Kickoff.pdf", -30, 10, 0),
				("Weekly Standup - Team Updates", "Standup_Notes.docx", -27, 11, 0),
				("Client Call - Requirements Discussion", "Client_Notes.pdf", -25, 12, 15),
				("Architecture Review - Module Design", "Architecture_Review.pptx", -22, 15, 0),
				("Sprint Planning - MOM Enhancements", "Sprint_Planning.xlsx", -20, 16, 30),
				("Design Review - UI Screens", "Design_Review.pdf", -18, 14, 0),
				("Project Review - Milestone 1", "Review_M1.pptx", -16, 15, 30),
				("Vendor Discussion - Hosting", "Vendor_Hosting.pdf", -14, 11, 30),
				("Security Review - Access & Roles", "Security_Checklist.docx", -12, 14, 0),
				("Incident Review - API Timeout", "Incident_Report.docx", -10, 17, 0),
				("Training Session - SQL Basics", "SQL_Training.pdf", -8, 10, 30),
				("Operational Review - Weekly Metrics", "Ops_Metrics.xlsx", -6, 13, 0),
				("Sprint Review - Demo", "Sprint_Review.pptx", -5, 16, 0),
				("Retrospective - Sprint Wrap-up", "Retro_Notes.docx", -4, 16, 30),
				("One-on-One - Progress Check", "OneOnOne_Notes.docx", -3, 18, 0),
				("Townhall - Monthly Updates", "Townhall_Agenda.pdf", -1, 17, 0),
				("Client Onboarding - Setup", "Onboarding_Checklist.pdf", 1, 11, 0),
				("Budget Review - Q1", "Budget_Q1.xlsx", 3, 13, 0),
				("Release Planning - v1.0", "Release_Plan_v1.pdf", 5, 15, 0),
				("Stakeholder Meeting - Roadmap", "Roadmap_Notes.docx", 7, 12, 0),
				("Training Session - Git Workflow", "Git_Training.pdf", 9, 10, 0),
				("Compliance Training - Policy", "Compliance_Policy.pdf", 11, 14, 30),
				("QBR - Client Review", "QBR_Summary.pptx", 14, 11, 0),
				("Escalation Call - Priority Bug", "Bug_Escalation.docx", 16, 19, 0),
				("Project Review - Milestone 2", "Review_M2.pptx", 20, 15, 0)
			];

			var i = 0;
			foreach (var item in seed)
			{
				if (existing.Count >= targetCount)
				{
					break;
				}

				var meetingDate = DateTime.Today.AddDays(item.OffsetDays).AddHours(item.Hour).AddMinutes(item.Minute);

				if (existing.Any(m => m.MeetingDate == meetingDate))
				{
					continue;
				}

				var dept = departments[i % departments.Count];
				var type = types[i % types.Count];
				var venue = venues[i % venues.Count];
				i++;

				using var cmd = new SqlCommand("dbo.PR_MOM_Meetings_Insert", connection)
				{
					CommandType = CommandType.StoredProcedure
				};

				cmd.Parameters.AddWithValue("@MeetingDate", meetingDate);
				cmd.Parameters.AddWithValue("@MeetingTypeID", type.MeetingTypeID);
				cmd.Parameters.AddWithValue("@DepartmentID", dept.DepartmentID);
				cmd.Parameters.AddWithValue("@MeetingVenueID", venue.MeetingVenueID);
				cmd.Parameters.AddWithValue("@MeetingDescription", item.Description);
				cmd.Parameters.AddWithValue("@DocumentPath", $"docs/{item.DocFile}");
				cmd.ExecuteNonQuery();

				existing.Add(new Meeting { MeetingDate = meetingDate });
			}

			// Mark a few meetings as cancelled so the Active/Cancelled filter has data.
			try
			{
				var meetingsAfterInsert = LoadMeetingsBasic(connection)
					.OrderBy(m => m.MeetingDate)
					.Take(3)
					.ToList();

				foreach (var m in meetingsAfterInsert)
				{
					using var update = new SqlCommand(@"UPDATE dbo.MOM_Meetings
SET IsCancelled = 1,
	CancellationDateTime = ISNULL(CancellationDateTime, GETDATE()),
	CancellationReason = ISNULL(NULLIF(CancellationReason, ''), 'Cancelled due to schedule conflict'),
	Modified = GETDATE()
WHERE MeetingID = @MeetingID", connection);
					update.Parameters.AddWithValue("@MeetingID", m.MeetingID);
					update.ExecuteNonQuery();
				}
			}
			catch
			{
				// Ignore if table name/schema differs.
			}
		}

		private static void SeedMeetingMembers(SqlConnection connection, int targetCount)
		{
			var meetings = LoadMeetingsBasic(connection)
				.OrderBy(m => m.MeetingDate)
				.ToList();
			var staff = LoadStaff(connection);
			if (meetings.Count == 0 || staff.Count == 0)
			{
				return;
			}

			var existingCount = GetMeetingMemberCount(connection);
			if (existingCount >= targetCount)
			{
				return;
			}

			var created = 0;
			for (var meetingIndex = 0; meetingIndex < meetings.Count && (existingCount + created) < targetCount; meetingIndex++)
			{
				var meeting = meetings[meetingIndex];
				for (var staffOffset = 0; staffOffset < 3 && (existingCount + created) < targetCount; staffOffset++)
				{
					var staffMember = staff[(meetingIndex + staffOffset) % staff.Count];
					var isPresent = (staffOffset % 3) != 2;

					try
					{
						using var cmd = new SqlCommand("dbo.PR_MOM_MeetingMember_Insert", connection)
						{
							CommandType = CommandType.StoredProcedure
						};

						cmd.Parameters.AddWithValue("@MeetingID", meeting.MeetingID);
						cmd.Parameters.AddWithValue("@StaffID", staffMember.StaffID);
						cmd.Parameters.AddWithValue("@IsPresent", isPresent);
						cmd.Parameters.AddWithValue("@Remarks", isPresent ? "Present" : "Absent");
						cmd.ExecuteNonQuery();

						created++;
					}
					catch
					{
						// Ignore duplicates / FK errors.
					}
				}
			}
		}

		private static List<Department> LoadDepartments(SqlConnection connection)
		{
			var departments = new List<Department>();
			using var command = new SqlCommand("PR_MOM_Department_SelectAll", connection)
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

		private static List<MeetingType> LoadMeetingTypes(SqlConnection connection)
		{
			var types = new List<MeetingType>();
			using var command = new SqlCommand("PR_MOM_MeetingType_SelectAll", connection)
			{
				CommandType = CommandType.StoredProcedure
			};
			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				types.Add(new MeetingType
				{
					MeetingTypeID = reader.GetInt32(reader.GetOrdinal("MeetingTypeID")),
					MeetingTypeName = reader.GetString(reader.GetOrdinal("MeetingTypeName"))
				});
			}
			reader.Close();
			return types;
		}

		private static List<MeetingVenue> LoadMeetingVenues(SqlConnection connection)
		{
			var venues = new List<MeetingVenue>();
			using var command = new SqlCommand("PR_MOM_MeetingVenue_SelectAll", connection)
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

		private static List<Staff> LoadStaff(SqlConnection connection)
		{
			var staffMembers = new List<Staff>();
			using var command = new SqlCommand("dbo.PR_MOM_Staff_SelectAll", connection)
			{
				CommandType = CommandType.StoredProcedure
			};
			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				staffMembers.Add(new Staff
				{
					StaffID = reader.GetInt32(reader.GetOrdinal("StaffID")),
					StaffName = reader.GetString(reader.GetOrdinal("StaffName"))
				});
			}
			reader.Close();
			return staffMembers;
		}

		private static List<Meeting> LoadMeetingsBasic(SqlConnection connection)
		{
			var meetings = new List<Meeting>();
			using var command = new SqlCommand("dbo.PR_MOM_Meetings_SelectAll", connection)
			{
				CommandType = CommandType.StoredProcedure
			};
			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				meetings.Add(new Meeting
				{
					MeetingID = reader.GetInt32(reader.GetOrdinal("MeetingID")),
					MeetingDate = reader.GetDateTime(reader.GetOrdinal("MeetingDate"))
				});
			}
			reader.Close();
			return meetings;
		}

		private static int GetMeetingMemberCount(SqlConnection connection)
		{
			try
			{
				using var cmd = new SqlCommand("dbo.PR_MOM_MeetingMember_SelectAll", connection)
				{
					CommandType = CommandType.StoredProcedure
				};
				using var reader = cmd.ExecuteReader();
				var count = 0;
				while (reader.Read())
				{
					count++;
				}
				reader.Close();
				return count;
			}
			catch
			{
				return 0;
			}
		}
	}
}
