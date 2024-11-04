using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ResultModels
{
    public class ResultModel
    {
        public bool Success { get; private set; }
        public string ErrorMessage { get; private set; }

        private ResultModel(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public static ResultModel Ok()
        {
            return new ResultModel(true, null);
        }

        public static ResultModel Error(string errorMessage)
        {
            return new ResultModel(false, errorMessage);
        }

    }
}
