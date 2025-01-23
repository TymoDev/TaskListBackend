using Core.DTO.UserDTO;
using Core.Interfaces.Logging;
using Core.Interfaces.Providers;
using Core.Interfaces.Repositories;
using Core.ResultModels;
using Core.ValidationModels.User;
using Infrastracture.Logic;

namespace Aplication.Services.User
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IUserRepository repository;
        private readonly IPasswordHasher hasher;
        private readonly IJwtProvider jwtProvider;
        private readonly IAppLogger logger;

        public UserAuthService(IUserRepository repository, IPasswordHasher hasher, IJwtProvider jwtProvider, IAppLogger logger)
        {
            this.repository = repository;
            this.hasher = hasher;
            this.jwtProvider = jwtProvider;
            this.logger = logger;
        }

        public async Task<LoginResultModel> Register(Guid id, RegisterUserRequest request)
        {
            logger.Information($"Registering user with email: {request.Email}");

            var userModelResult = UsernameModel.Create(request.Username);
            if (!userModelResult.Success)
            {
                logger.Error($"Username validation failed: {userModelResult.ErrorMessage}");
                return LoginResultModel.Error(userModelResult.ErrorMessage);
            }

            var result = UserPasswordModel.Create(request.Password);
            if (!result.Success)
            {
                logger.Error($"Password validation failed: {result.ErrorMessage}");
                return LoginResultModel.Error(result.ErrorMessage);
            }

            var hashedPassword = hasher.Generate(request.Password);
            logger.Information($"Creating user with ID: {id} and username: {request.Username}");
            await repository.CreateUser(new UserRequestHash(id, request.Username, request.Email, hashedPassword));

            var user = await repository.GetUserById(id);
            var token = jwtProvider.GenerateAuthenticateToken(user);
            logger.Information($"User with ID: {id} registered successfully");

            return LoginResultModel.Ok(token);
        }

        public async Task<LoginResultModel> Login(LoginUserRequest request)
        {
            logger.Information($"Logging in user with login: {request.Login}");

            var user = await repository.GetUserByEmailOrUsername(request.Login);
            if (user == null)
            {
                logger.Warning($"Login failed: User with login {request.Login} not found");
                return LoginResultModel.Error("Invalid login credentials");
            }

            bool result = hasher.Verify(request.Password, user.Password.ToString());
            if (!result)
            {
                logger.Warning("Login failed: Incorrect password");
                return LoginResultModel.Error("Incorrect password");
            }

            var token = jwtProvider.GenerateAuthenticateToken(user);
            logger.Information($"User with ID: {user.Id} logged in successfully");

            return LoginResultModel.Ok(token);
        }
        public async Task<ResultModel> Logout()
        {
            logger.Information("Logging out user");
            //Additional logout logic
            logger.Information("User successfully logged out");
            return ResultModel.Ok();
        }
    }
}
