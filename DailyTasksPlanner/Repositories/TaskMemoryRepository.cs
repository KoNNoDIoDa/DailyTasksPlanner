using DailyTasksPlanner.Core.Interfaces;
using DailyTasksPlanner.Core.Models;

namespace DailyTasksPlanner.Core.Repositories
{
    public class TaskMemoryRepository : ITaskRepository
    {
        private int _globalId = 1;
        private readonly Dictionary<int, TaskItem> _tasks = new ();

        public void Add(TaskItem task)
        {
            task.Id = _globalId++;
            _tasks[task.Id] = task;
        }
        public void Edit(TaskItem task) => _tasks[task.Id] = task;
        public void Delete(int Id) => _tasks.Remove(Id);
        public void CompleteTask(int Id) => _tasks[Id].MarkAsCompleted();
        public TaskItem GetById(int Id) => _tasks.TryGetValue(Id, out var task) ? task : null;
        public IEnumerable<TaskItem> GetAll() => _tasks.Values;
        public string GetTaskById(int Id) => _tasks[Id].ToString();
    }
}