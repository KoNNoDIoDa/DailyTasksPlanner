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

        public TaskItem(string name, string descrription, int priority, DateTime start, DateTime finish)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("�������� ������ �� ����� ���� ������");
            if (finish < start) throw new ArgumentException("���� ��������� �� ����� ���� ������ ���� ������");

            Name = name;
            Description = descrription;
            Priority = priority;
            Start = start;
            Finish = finish;
        }
        public void Update(string name, string descrription, int priority, DateTime start, DateTime finish)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("�������� ������ �� ����� ���� ������");
            if (finish < start) throw new ArgumentException("���� ��������� �� ����� ���� ������ ���� ������");

            Name = name;
            Description = descrription;
            Priority = priority;
            Start = start;
            Finish = finish;
        }
        public void MarkAsCompleted() => IsCompleted = true;
        public override string ToString()
        {
            return $"ID: {Id}, ��������: {Name}, ��������: {Description}, ���������: {Priority}, ������: {Start:yyy-MM-dd}, ���������: {Finish:yy-MM-dd}, ����������: {IsCompleted}";
        }
    }
}