using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class PermissionEntity
    {
        public required int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public  ICollection<RoleEntity> Roles { get; set; } = [];
    }
}
