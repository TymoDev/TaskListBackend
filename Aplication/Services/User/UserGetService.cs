using Core.DTO.UserDTO;
using Core.Enums;
using Core.Interfaces.Logging;
using Core.Interfaces.Repositories;

namespace Aplication.Services
{
    public class UserGetService : IUserGetService
    {
        private readonly IUserRepository repository;
        private readonly IAppLogger logger;

        public UserGetService(IUserRepository repository, IAppLogger logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<List<UserPasswordDto>> GetUsers()
        {
            logger.Information("Fetching all users");
            var users = await repository.GetUsers();
            logger.Information($"Fetched {users.Count} users");
            return users;
        }

        public async Task<UserPasswordDto?> GetUser(Guid id)
        {
            logger.Information($"Fetching user with ID: {id}");
            var responce = await repository.GetUserById(id);
            if (responce == null)
            {
                logger.Warning($"User with ID: {id} not found");
                return null;
            }
            logger.Information($"User with ID: {id} fetched successfully");
            return responce;
        }

        public async Task<UserPasswordDto?> GetUserByEmailOrLogin(string login)
        {
            logger.Information($"Fetching user with login or email: {login}");
            var responce = await repository.GetUserByEmailOrUsername(login);
            if (responce == null)
            {
                logger.Warning($"User with login or email: {login} not found");
                return null;
            }
            logger.Information($"User with login or email: {login} fetched successfully");
            return responce;
        }

        public async Task<HashSet<Permission>> GetPermissions(Guid userId)
        {
            logger.Information($"Fetching permissions for user with ID: {userId}");
            var permissions = await repository.GetUserPermissions(userId);
            logger.Information($"Fetched {permissions.Count} permissions for user with ID: {userId}");
            return permissions;
        }
    }
}
