namespace MOM_Project.Models
{
	public class DashboardViewModel
	{
		public int TotalMeetingsThisMonth { get; set; }
		public int TotalMeetingsCancelledThisMonth { get; set; }
		public int TotalStaff { get; set; }
		public int TotalDepartments { get; set; }

		public List<UpcomingMeetingRow> UpcomingMeetings { get; set; } = new();
		public List<string> ChartCategories { get; set; } = new();
		public List<int> ChartMeetingsHeld { get; set; } = new();
		public List<int> ChartMeetingsCancelled { get; set; } = new();
	}

	public class UpcomingMeetingRow
	{
		public int MeetingID { get; set; }
		public string? MeetingDescription { get; set; }
		public DateTime MeetingDate { get; set; }
		public string? VenueName { get; set; }
		public string? DepartmentName { get; set; }
		public bool IsCancelled { get; set; }
	}
}
