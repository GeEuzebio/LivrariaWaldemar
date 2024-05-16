namespace LibraryApp.Models
{
    public class Scheduling
    {
        public int Id { get; set; }
        public DateTime? Day { get; set;}
        public List<ClassRoom?> ClassRoom { get; set;} = [null, 
                                                          null, 
                                                          null, 
                                                          null, 
                                                          null, 
                                                          null, 
                                                          null, 
                                                          null, 
                                                          null];
        public List<Professionals?> Professor { get; set;} = [null, 
                                                              null, 
                                                              null, 
                                                              null, 
                                                              null, 
                                                              null, 
                                                              null, 
                                                              null, 
                                                              null];
        public List<string> Hours { get; set;} = ["07:10 - 08:00", 
                                                  "08:00 - 08:50", 
                                                  "08:50 - 09:40", 
                                                  "10:00 - 10:50", 
                                                  "10:50 - 11:40", 
                                                  "13:10 - 14:00", 
                                                  "14:00 - 14:50", 
                                                  "15:10 - 16:00", 
                                                  "16:00 - 16:50"];
        public List<bool> IsScheduled { get; set; } = [false, 
                                                       false, 
                                                       false, 
                                                       false, 
                                                       false, 
                                                       false, 
                                                       false, 
                                                       false, 
                                                       false];
    }
}