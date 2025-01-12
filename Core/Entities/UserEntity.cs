using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class UserEntity
    {
        public required Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public  ICollection<RoleEntity> Roles { get; set; } = [];
        
        public List<TaskEntity>? Tasks { get; set; }
    }
}
