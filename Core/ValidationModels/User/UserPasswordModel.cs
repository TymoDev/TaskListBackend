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
                return ResultModel.Error("Password can't be null");
            }

            if (password.Length < 10)
            {
                return ResultModel.Error("Weak password");
            }
            return ResultModel.Ok();
        }
    }
}