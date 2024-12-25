using Core.DTO.UserDTO;
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

        public UserAuthService(IUserRepository repository, IPasswordHasher hasher, IJwtProvider jwtProvider)
        {
            this.repository = repository;
            this.hasher = hasher;
            this.jwtProvider = jwtProvider;
        }
        public async Task<LoginResultModel> Register(Guid id,RegisterUserRequest request)
        {
            var userModelResult = UsernameModel.Create(request.Username);
            if (!userModelResult.Success)
            {
                return LoginResultModel.Error(userModelResult.ErrorMessage);
            }
            var result = UserPasswordModel.Create(request.Password);
            if (!result.Success)
            {
                return LoginResultModel.Error(result.ErrorMessage);
            }

            var hashedPassword = hasher.Generate(request.Password);
            await repository.CreateUser(new UserRequestHash(id, request.Username, request.Email, hashedPassword));
            var user = await repository.GetUserById(id);
            var token = jwtProvider.GenerateAuthenticateToken(user);
            return LoginResultModel.Ok(token);
        }

        public async Task<LoginResultModel> Login(LoginUserRequest request)
        {
            
            var user = await repository.GetUserByEmailOrUsername(request.Login); 
            if (user == null)
            {
                return LoginResultModel.Error("BadRequest with data");
            }
            bool result = hasher.Verify(request.Password, user.Password.ToString());
            if (result == false)
            {
                return LoginResultModel.Error("Incorrect password");
            }
            var token = jwtProvider.GenerateAuthenticateToken(user);
            return LoginResultModel.Ok(token);

        }
    }
}
