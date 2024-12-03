using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DailyTasksPlanner.Core.Models;

namespace DailyTasksPlanner.Core.Interfaces
{
    public interface ISerializerService
    {
        void Serialize();
        IEnumerable<TaskItem> Deserialize();
    }
}
