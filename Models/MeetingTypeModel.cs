namespace MOM_Project.Models
{
    public class MeetingType
    {
        public int MeetingTypeID { get; set; }
        public string MeetingTypeName { get; set; }
        public string? Remarks { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
