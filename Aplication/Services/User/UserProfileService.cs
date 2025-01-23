/*using Core.DTO.UserDTO;
using Core.Interfaces.Logging;
using Core.Interfaces.Repositories;
using Persistance.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services.User
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository repository;
        private readonly IAppLogger logger;

        public UserProfileService(IUserProfileRepository repository, IAppLogger logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<UserProfileDto?> GetUserProfile(Guid id)
        {
            logger.Information($"Fetching user with ID: {id}");
            var responce = await repository.GetUserProfile(id);
            if (responce == null)
            {
                logger.Warning($"User with ID: {id} not found");
                return null;
            }
            logger.Information($"User with ID: {id} fetched successfully");
            return responce;
        }
    }
}
*/