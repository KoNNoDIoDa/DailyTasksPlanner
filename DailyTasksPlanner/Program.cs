namespace DailyTasksPlanner
{
    internal class Program
    {
        private static readonly TasksManager TasksManager = new();
        static void Main(string[] args)
        {
            
            while (true)
            {
                
                try
                {
                    DisplayMenu();
                    if (!int.TryParse(Console.ReadLine(), out int action) || !Enum.IsDefined(typeof(ActionType), action))
                    {
                        Console.WriteLine("Ошибка. Введите число от 1 до 6.");
                        continue;
                    }
                    if ((ActionType)action == ActionType.Exit) break;
                    ExecuteAction((ActionType)action);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }
            }

        }
        private static void ExecuteAction(ActionType action)
        {
            switch (action)
            {
                case ActionType.CreateTask:
                    AddTask();
                    break;
                case ActionType.EditTask:
                    EditTask();
                    break;
                case ActionType.DeleteTask:
                    DeleteTask();
                    break;
                case ActionType.CompleteTask:
                    FinishTask();
                    break;
                case ActionType.ShowAllTasks:
                    ShowAllTasks();
                    break;
            }
        }
        private static void DisplayMenu()
        {
            Console.WriteLine("\nВыберите дейтсие");
            Console.WriteLine("1. Создать задачу");
            Console.WriteLine("2. Редактировать задачу");
            Console.WriteLine("3. Удалить задачу");
            Console.WriteLine("4. Завершить задачу");
            Console.WriteLine("5. Показать все задачи");
            Console.WriteLine("6. Выход");
        }
        
        static void AddTask()
        {
            string name = GetValidatedInput("Введите название задачи", input => !string.IsNullOrWhiteSpace(input));
            string description = GetValidatedInput("Введите описание задачи", input => true);
            int priority = GetValidatedIntInput("Введите приоритет задачи 1 - наивысший, 3 - наименьший", input => int.TryParse(input, out var value) && value is >= 1 and <= 3);
            DateTime start = GetValidatedDateTime("Введите дату, на которую назначить задачу");
            DateTime finish = GetValidatedDateTime("Введите дату, до которой задача должна быть вполнена", start);

            TasksManager.CreateTask(new Task(name, description, priority, start, finish));
            Console.WriteLine("Задача успешно создана");
        }
        static void EditTask()
        {
            if (!TryGetTaskbyId(out var task)) return;

            string name = GetValidatedInput("Введите название задачи", input => !string.IsNullOrWhiteSpace(input));
            string description = GetValidatedInput("Введите описание задачи", input => true);
            int priority = GetValidatedIntInput("Введите приоритет задачи 1 - наивысший, 3 - наименьший", input => int.TryParse(input, out var value) && value is >= 1 and <= 3);
            DateTime start = GetValidatedDateTime("Введите дату, на которую назначить задачу");
            DateTime finish = GetValidatedDateTime("Введите дату, до которой задача должна быть вполнена", start);

            task.Update(name, description, priority, start, finish);
            Console.WriteLine("Задача успешно изменена");
        }
        static void DeleteTask()
        {
            if (!TryGetTaskbyId(out var task)) return;

            if (RightTaskPicked(task.Id))
            {
                TasksManager.DeleteTask(task.Id);
                Console.WriteLine("Задача успешно удалена");
            }
        }
        static void FinishTask()
        {
            if (!TryGetTaskbyId(out var task)) return;

            if (RightTaskPicked(task.Id))
            {
                TasksManager.CompleteTask(task.Id);
                Console.WriteLine("Задача успешно завершена");
            }
        }
        static void ShowAllTasks()
        {
            var tasks = TasksManager.GetAllTasks();
            if (!tasks.Any())
            {
                Console.WriteLine("Нет фактивных задач");
                return;
            }
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }
        }
        private static string GetValidatedInput(string prompt, Func<string, bool> validator)
        {
            Console.WriteLine(prompt);
            string input;
            do
            {
                input = Console.ReadLine()?.Trim() ?? string.Empty;
                if (!validator(input))
                {
                    Console.WriteLine("Ошибка: Введено некорректное значение. Попробуйте ещё раз.");
                }
            }
            while (!validator(input));

            return input;
        }
        private static int GetValidatedIntInput(string prompt, Func<string, bool> validator)
        {
            string input = GetValidatedInput(prompt, validator);
            return int.Parse(input);
        }
        private static DateTime GetValidatedDateTime(string prompt, DateTime? startDate = null)
        {
            Console.WriteLine(prompt);
            DateTime date;
            while (!DateTime.TryParse(Console.ReadLine(), out date) || (startDate.HasValue && startDate > date))
            {
                Console.WriteLine("Ошибка: введите корректную дату (ГГГ-ММ-ДД), которая не раньше даты начала.");
            }
            return date;
        }
        private static bool TryGetTaskbyId (out Task task)
        {
            task = null;
            int id = GetValidatedIntInput("Введите ID задачи: ", input => int.TryParse(input, out _));
            task = TasksManager.GetTaskById(id);
            if (task != null) return true;

            Console.WriteLine("Задача с таким ID не найдена");
            return false;
        }
        private static bool RightTaskPicked(int id)
        {
            Task task = TasksManager.GetTaskById(id);
            Console.WriteLine(task.ToString());
            string inp = GetValidatedInput("Это нужная вам задача Да/Нет", input => !string.IsNullOrWhiteSpace(input) && (input.Equals("да", StringComparison.OrdinalIgnoreCase) || input.Equals("нет", StringComparison.OrdinalIgnoreCase)));
            return inp == "да" ? true : false;
        }
    }
    public enum ActionType
    {
        CreateTask = 1,
        EditTask,
        DeleteTask,
        CompleteTask,
        ShowAllTasks,
        Exit
    }
    class TasksManager
    {
        private int _globalId = 1;
        private readonly Dictionary<int, Task> _tasks = new();
        public void CreateTask(Task task)
        {
            task.Id = _globalId++;
            _tasks[task.Id] = task;
        }
       public void DeleteTask(int id) => _tasks.Remove(id);
        public void CompleteTask(int id)
        {
            if(_tasks.TryGetValue(id, out var task))
            {
                task.MarkAsCompleted();
            }
        }
        public Task GetTaskById(int id) => _tasks.TryGetValue(id, out var task) ? task : null;
        public IEnumerable<Task> GetAllTasks() => _tasks.Values;
    }
    class Task
    {
        public int Id { get; set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Priority { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime Finish { get; private set; }
        public bool IsCompleted { get; private set; }

        public Task(string name, string descrription, int priority, DateTime start, DateTime finish)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("название задачи не может быть пустым");
            if (finish < start) throw new ArgumentException("Дата окончания не может быть раньше даты начала");

            Name = name;
            Description = descrription;
            Priority = priority;
            Start = start;
            Finish = finish;
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
