using Core.Interfaces.Logging;
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
        private readonly IAppLogger logger;

        public UserResetPasswordService(ICodeGenerator codeGenerator, ICacher cacher, IEmailSender emailSender, IAppLogger logger)
        {
            this.codeGenerator = codeGenerator;
            this.cacher = cacher;
            this.emailSender = emailSender;
            this.logger = logger;
        }

        public async Task<ResultModel> ResetPasswordNotify(string email)
        {
            logger.Information($"Starting password reset notification for email: {email}");

            int code = codeGenerator.GenerateSecureCode();
            logger.Information($"Generated secure code for email: {email}");

            await cacher.SetCode(email, code);
            logger.Information($"Cached secure code for email: {email}");

            emailSender.SendMail(email, "Reset code mail", code.ToString());
            logger.Information($"Sent reset code email to: {email}");

            return ResultModel.Ok();
        }

        public async Task<ResultModel> ResetPasswordVerify(string email, int code)
        {
            logger.Information($"Verifying reset code for email: {email}");

            try
            {
                var result = await cacher.GetCode(email);
                if (result == null)
                {
                    logger.Warning($"No code found in cache for email: {email}");
                    return ResultModel.Error("Incorrect code");
                }

                bool verify = result == code;
                if (verify)
                {
                    logger.Information($"Code verification successful for email: {email}");
                    return ResultModel.Ok();
                }
                else
                {
                    logger.Warning($"Code verification failed for email: {email}");
                    return ResultModel.Error("Incorrect code");
                }
            }
            catch (Exception ex)
             {
                logger.Error($"Unexpected exception during reset code verification for email: {email}: {ex.Message}");
                return ResultModel.Error($"Unexpected exception: {ex.Message}");
            }
        }
    }

}

