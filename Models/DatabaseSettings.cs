namespace APIStarted.Models
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string DepartmentCollection { get; set; }
        public string PositionCollection { get; set; }
        public string MemberCollection { get; set; }

    }
    public interface IDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string DepartmentCollection { get; set; }
        string PositionCollection { get; set; }
        string MemberCollection { get; set; }
    }
}