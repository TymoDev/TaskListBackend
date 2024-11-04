using Core.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Core.Models.TaskModel
{
    public class TaskModel
    {
        private TaskModel()
        {
        }

        public static ResultModel Create(string taskName, string taskStatus)
        {
            if (String.IsNullOrEmpty(taskName) || String.IsNullOrEmpty(taskStatus))
            {
                return ResultModel.Error("Task name and status is requared");
            }
            return ResultModel.Ok();
        }
    }
}
