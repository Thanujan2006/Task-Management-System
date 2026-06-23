namespace Task_Management_System.DTOs
{
    public class TaskItemResponseDto
    {
        
            public int TaskId { get; set; }
            public string Title { get; set; }
            public string? Description { get; set; }
            public DateTime? DueDate { get; set; }
            public string Status { get; set; }

            public int UserId { get; set; }

            public string UserName { get; set; }

    }
}
