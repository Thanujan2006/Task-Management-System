using Task_Management_System.Models;
using Task_Management_System.Repositories;

namespace Task_Management_System.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _task;

        public TaskService(ITaskRepository task) {

            _task = task;


        }
        public List<TaskItem> GetAllTasks(){

            var list1 = _task.GetAllTasks();
            return list1;

          }

        public TaskItem GetTaskById(int taskId)
        {

            var list= _task.GetTaskById(taskId);
            return list;
        }







    }
}
