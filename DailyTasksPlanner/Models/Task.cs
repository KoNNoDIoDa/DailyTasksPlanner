using System.Text.Json.Serialization;

namespace DailyTasksPlanner.Core.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Priority { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime Finish { get; private set; }
        public bool IsCompleted { get; private set; }

        [JsonConstructor]
        public TaskItem(int id, string name, string description, int priority, DateTime start, DateTime finish, bool isCompleted)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("название задачи не может быть пустым");
            if (finish < start) throw new ArgumentException("Дата окончания не может быть раньше даты начала");

            Id = id;
            Name = name;
            Description = description;
            Priority = priority;
            Start = start;
            Finish = finish;
            IsCompleted = isCompleted;
        }
        public void Update(string name, string descrription, int priority, DateTime start, DateTime finish)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("название задачи не может быть пустым");
            if (finish < start) throw new ArgumentException("Дата окончания не может быть раньше даты начала");

            Name = name;
            Description = descrription;
            Priority = priority;
            Start = start;
            Finish = finish;
        }
        public void MarkAsCompleted() => IsCompleted = true;
        public override string ToString()
        {
            return $"ID: {Id}, Название: {Name}, Описание: {Description}, Приоритет: {Priority}, Начало: {Start:yyy-MM-dd}, Окончание: {Finish:yy-MM-dd}, Заверешена: {IsCompleted}";
        }
    }
}