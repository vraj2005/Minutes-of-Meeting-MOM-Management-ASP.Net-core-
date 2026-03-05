using System.ComponentModel.DataAnnotations;

namespace MOM_Project.Models
{
	public class FillAttendanceViewModel
	{
		public List<Meeting> Meetings { get; set; } = new();
		public int? SelectedMeetingId { get; set; }
		public Meeting? Meeting { get; set; }

		public List<FillAttendanceStaffRow> StaffRows { get; set; } = new();
	}

	public class FillAttendanceStaffRow
	{
		public int StaffID { get; set; }
		public string? StaffName { get; set; }
		public string? DepartmentName { get; set; }
		public bool IsPresent { get; set; }
		public string? Remarks { get; set; }
	}

	public class FillAttendancePostModel
	{
		[Required]
		public int MeetingID { get; set; }

		public int[] PresentStaffIds { get; set; } = [];
	}
}
