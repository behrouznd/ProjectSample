namespace ConsoleAppCore.Entities
{

    public interface IAuditable02 { }

    public class Course : IAuditable02
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }
}
