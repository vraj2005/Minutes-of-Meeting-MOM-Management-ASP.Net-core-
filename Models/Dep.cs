namespace MOM_Project.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string? Remarks { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
