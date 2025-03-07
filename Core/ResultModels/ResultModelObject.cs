using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ResultModels
{
    public class ResultModelObject<T>
    {
        public bool Success { get; private set; }
        public string ErrorMessage { get; private set; }
        public T Data { get; private set; }

        private ResultModelObject(bool success, string errorMessage, T data)
        {
            Success = success;
            ErrorMessage = errorMessage;
            Data = data;
        }

        public static ResultModelObject<T> Ok(T data)
        {
            return new ResultModelObject<T>(true, null, data);
        }

        public static ResultModelObject<T> Ok()
        {
            return new ResultModelObject<T>(true, null, default(T));
        }

       
        public static ResultModelObject<T> Error(string errorMessage)
        {
            return new ResultModelObject<T>(false, errorMessage, default(T));
        }     
    }
}
