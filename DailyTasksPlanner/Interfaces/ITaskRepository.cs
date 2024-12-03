using DailyTasksPlanner.Core.Models;

namespace DailyTasksPlanner.Core.Interfaces
{
    public interface ITaskRepository
    {
        void Add(TaskItem task);
        void Edit(TaskItem task);
        void Delete(int Id);
        void CompleteTask(int Id);
        TaskItem GetById(int Id);
        IEnumerable<TaskItem> GetAll();
        string GetTaskById(int Id);
        void RefillTasks(IEnumerable<TaskItem> tasks);
    }
}