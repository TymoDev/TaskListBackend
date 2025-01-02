using Core.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValidationModels.User
{
    public class UserPasswordModel
    {
        private UserPasswordModel()
        {
        }

        public static ResultModel Create(string password)
        {
            if (password == null)
            {
                return ResultModel.Error("Password is required");
            }

            if (password.Length < 10)
            {
                return ResultModel.Error("Weak password");
            }
            if(password.Length > 50)
            {
                return ResultModel.Error("Password can not be longer than 50 sybols");
            }
            return ResultModel.Ok();
        }
    }
}