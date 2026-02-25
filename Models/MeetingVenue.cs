namespace MOM_Project.Models
{
    public class MeetingVenue
    {
        public int MeetingVenueID { get; set; }
        public string MeetingVenueName { get; set; }
        public string? Remarks { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
