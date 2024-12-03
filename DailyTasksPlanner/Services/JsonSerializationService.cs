using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using DailyTasksPlanner.Core.Interfaces;
using DailyTasksPlanner.Core.Models;
using System.Collections;

namespace DailyTasksPlanner.Core.Services
{
    internal class JsonSerializationService : ISerializerService
    {
        ITaskRepository _repository;

        public JsonSerializationService(ITaskRepository repository)
        {
            _repository = repository;
        }
        public void Serialize()
        {
            using (FileStream fs = new FileStream("C:\\Users\\mushr\\Documents\\save.json", FileMode.OpenOrCreate))
            {
                
                var options = new JsonSerializerOptions()
                {
                    IncludeFields = true,
                    PropertyNameCaseInsensitive = true
                };
                IEnumerable<TaskItem> _tasks = _repository.GetAll();
                JsonSerializer.Serialize(fs, _tasks);
                Console.WriteLine("Сохранено");
            }
        }
        public IEnumerable<TaskItem> Deserialize()
        {
            try
            {
                using (FileStream fs = new FileStream("C:\\Users\\mushr\\Documents\\save.json", FileMode.OpenOrCreate))
                {
                    var options = new JsonSerializerOptions()
                    {
                        IncludeFields = true,
                        PropertyNameCaseInsensitive = true
                    };
                    IEnumerable<TaskItem>? _tasks = JsonSerializer.Deserialize<IEnumerable<TaskItem>>(fs, options);
                    return _tasks;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Enumerable.Empty<TaskItem>();
                
            }
        }
    }
}
