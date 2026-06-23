namespace Task_Management_System.DTOs
{
    public class UserWithTasksDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public List<TaskItemResponseDto> Tasks { get; set; }
    }
}
