using Task_Management_System.Models;

namespace Task_Management_System.Repositories
{
    public interface ITaskRepository
    {

        public List<TaskItem> GetAllTasks();
        public TaskItem GetTaskById(int taskId);

        public TaskItem SearchTasks(string Title);
        public List<TaskItem> AddTask(int TaskId, string Title, string Description, string Status, int UserId);
        public List<TaskItem> UpdateTask(int TaskId, string Title, string Description, string Status, int UserId);
        public TaskItem ChangeStatus(string Status, int TaskId);
        public void DeleteTask(string Status, int TaskId);


    }
}
