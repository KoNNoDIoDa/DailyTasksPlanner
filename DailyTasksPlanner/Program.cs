using DailyTasksPlanner.Core.Interfaces;
using DailyTasksPlanner.Core.Repositories;
using DailyTasksPlanner.Core.Services;

namespace DailyTasksPlanner
{
    internal class Program
    {
        private static readonly ITaskRepository repository = new TaskMemoryRepository();
        private static readonly TaskService _taskService = new(repository);
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

            _taskService.AddTask(name, description, priority, start, finish);
            Console.WriteLine("Задача успешно создана");
        }
        static void EditTask()
        {
            if (!TryGetTaskbyId(out var id)) return;

            string name = GetValidatedInput("Введите название задачи", input => !string.IsNullOrWhiteSpace(input));
            string description = GetValidatedInput("Введите описание задачи", input => true);
            int priority = GetValidatedIntInput("Введите приоритет задачи 1 - наивысший, 3 - наименьший", input => int.TryParse(input, out var value) && value is >= 1 and <= 3);
            DateTime start = GetValidatedDateTime("Введите дату, на которую назначить задачу");
            DateTime finish = GetValidatedDateTime("Введите дату, до которой задача должна быть вполнена", start);

            _taskService.EditTask(id, name, description, priority, start, finish);
            Console.WriteLine("Задача успешно изменена");
        }
        static void DeleteTask()
        {
            if (!TryGetTaskbyId(out var id)) return;

            if (RightTaskPicked(id))
            {
                _taskService.DeleteTask(id);
                Console.WriteLine("Задача успешно удалена");
            }
        }
        static void FinishTask()
        {
            if (!TryGetTaskbyId(out var id)) return;

            if (RightTaskPicked(id))
            {
                _taskService.FinishTask(id);
                Console.WriteLine("Задача успешно завершена");
            }
        }
        static void ShowAllTasks()
        {
            var tasks = _taskService.ShowAllTasks();
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
        private static bool TryGetTaskbyId(out int id)
        {
            id = GetValidatedIntInput("Введите ID задачи: ", input => int.TryParse(input, out _));
            if (_taskService.CheckId(id)) return true;

            Console.WriteLine("Задача с таким ID не найдена");
            return false;
        }
        private static bool RightTaskPicked(int id)
        {
            Console.WriteLine(_taskService.GetTaskById(id));
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
}