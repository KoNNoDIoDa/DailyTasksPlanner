using DailyTasksPlanner.Core.Repositories;
using DailyTasksPlanner.Core.Models;
using DailyTasksPlanner.Core.Interfaces;
namespace DailyTasksPlanner.Core.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _repository;
        private readonly ISerializerService _serializerService;
        private delegate void UpdateHandler();
        event UpdateHandler Notify;

        public TaskService(ITaskRepository repository, ISerializerService serializer)
        {
            _repository = repository;
            _serializerService = serializer;
            Notify += _serializerService.Serialize;
            _repository.RefillTasks(_serializerService.Deserialize());
        }
        public void AddTask(string name, string description, int priority, DateTime start, DateTime finish)
        {
            _repository.Add(new TaskItem(0, name, description, priority, start, finish, false));
            Notify?.Invoke();
        }
        public void EditTask(int id, string name, string description, int priority, DateTime start, DateTime finish)
        {
            TaskItem task = _repository.GetById(id);
            task.Update(name, description, priority, start, finish);
            _repository.Edit(task);
            Notify?.Invoke();
        }
        public void DeleteTask(int Id)
        {
            _repository.Delete(Id);
            Notify?.Invoke();
        }
        public void FinishTask(int Id)
        {
            _repository.CompleteTask(Id);
            Notify?.Invoke();
        }
        public IEnumerable<TaskItem> ShowAllTasks() => _repository.GetAll();
        public bool CheckId(int id) => _repository.GetById(id) != null;
        public string GetTaskById(int id) => _repository.GetTaskById(id);
    }
}
