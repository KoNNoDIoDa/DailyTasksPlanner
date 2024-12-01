using DailyTasksPlanner.Core.Repositories;
using DailyTasksPlanner.Core.Models;
using DailyTasksPlanner.Core.Interfaces;
namespace DailyTasksPlanner.Core.Services
{
    public class TaskService
    {
        private ITaskRepository _repository;
        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }
        public void AddTask(string name, string description, int priority, DateTime start, DateTime finish) => _repository.Add(new TaskItem(name, description, priority, start, finish));
        public void EditTask(int id, string name, string description, int priority, DateTime start, DateTime finish)
        {
            TaskItem task = _repository.GetById(id);
            task.Update(name, description, priority, start, finish);
            _repository.Edit(task);
        }
        public void DeleteTask(int Id)
        {
            _repository.Delete(Id);
        }
        public void FinishTask(int Id) => _repository.CompleteTask(Id);
        public IEnumerable<TaskItem> ShowAllTasks() => _repository.GetAll();
        public bool CheckId(int id) => _repository.GetById(id) != null ? true : false;
        public string GetTaskById(int id) => GetTaskById(id);
    }
}
