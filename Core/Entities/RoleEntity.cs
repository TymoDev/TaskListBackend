using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class RoleEntity
    {
        public required int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public ICollection<PermissionEntity> Permissions { get; set; } = [];
        public ICollection<UserEntity> Users { get; set; } = [];
    }
}
