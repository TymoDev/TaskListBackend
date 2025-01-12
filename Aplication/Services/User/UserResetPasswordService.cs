using Core.ResultModels;
using Infrastracture.Caching;
using Infrastracture.EmailLogic;
using Infrastracture.Logic.CodesGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services.User
{
    public class UserResetPasswordService : IUserResetPasswordService
    {
        private readonly ICodeGenerator codeGenerator;
        private readonly ICacher cacher;
        private readonly IEmailSender emailSender;

        public UserResetPasswordService(ICodeGenerator codeGenerator, ICacher cacher,IEmailSender emailSender)
        {
            this.codeGenerator = codeGenerator;
            this.cacher = cacher;
            this.emailSender = emailSender;
        }
        public async Task<ResultModel> ResetPasswordNotify(string email)
        {
            int code = codeGenerator.GenerateSecureCode();
            await cacher.SetCode(email,code);
            emailSender.SendMail(email,"Reset code mail",code.ToString());
            return ResultModel.Ok();
        }
        public async Task<ResultModel> ResetPasswordVerify(string email,int code)
        {
            try
            {
                var result = await cacher.GetCode(email);
                if (result == null)
                {
                    return ResultModel.Error("Incorrect code");
                }
                bool verify = result == code;
                if (verify)
                {
                    return ResultModel.Ok();
                }
                else
                {
                    return ResultModel.Error("Incorrect code");
                }
            }
            catch(Exception ex)
            {
                return ResultModel.Error($"Unexpected exeption : {ex}");
            }
        }
    }
}
    
