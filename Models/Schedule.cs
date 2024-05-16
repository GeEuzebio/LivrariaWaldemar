namespace LibraryApp.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public DateTime Day { get; set; }
        public DateTime ClassHour { get; set; }
        public Professionals Professor { get; set; }
        public ClassRoom classRoom { get; set; }
    }
}