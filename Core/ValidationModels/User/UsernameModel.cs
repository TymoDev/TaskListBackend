using Core.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValidationModels.User
{
    public class UsernameModel
    {
        public const int Max_Username_Lenght = 30;
        public const int Min_Username_Lenght = 3;
        private UsernameModel()
        {
        }

        public static ResultModel Create(string username)
        {
            if (username.Length < Min_Username_Lenght || username.Length > Max_Username_Lenght)
            {
                return ResultModel.Error("Username can not be longer than 30 symlos, or lower than 3");
            }
            return ResultModel.Ok();
        }
    }
}
