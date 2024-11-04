using System;

namespace Core.ResultModels
{
    public class LoginResultModel
    {
        public bool Success { get; private set; }
        public string ErrorMessage { get; private set; }
        public string Token { get; private set; }  

        private LoginResultModel(bool success, string errorMessage, string token = null)
        {
            Success = success;
            ErrorMessage = errorMessage;
            Token = token; 
        }

        public static LoginResultModel Ok(string token)
        {
            return new LoginResultModel(true, null, token); 
        }

        public static LoginResultModel Error(string errorMessage)
        {
            return new LoginResultModel(false, errorMessage);
        }
    }
}
