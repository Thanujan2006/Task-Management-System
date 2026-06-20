namespace Task_Management_System.Models
{
    public class TaskItem
    {

        public int TaskId {  get; set; }
        public string Title {  get; set; }
        public string Description {  get; set; }
        public string Status {  get; set; }

        public DateTime CreatedDate; 
        public int UserId {  get; set; }
    }
}
