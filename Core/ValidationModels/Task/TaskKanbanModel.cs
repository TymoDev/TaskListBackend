using Core.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValidationModels.Task
{
    public class TaskKanbanModel
    {
        private TaskKanbanModel()
        {
            
        }
        public static ResultModel Create(string taskName)
        {
            if (String.IsNullOrEmpty(taskName))
            {
                return ResultModel.Error("Task name and status is requared");
            }
            return ResultModel.Ok();
        }
    }
}
