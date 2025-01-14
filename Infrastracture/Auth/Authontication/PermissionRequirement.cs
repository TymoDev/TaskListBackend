using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.Auth.Authontication
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(Permission[] permissions)
        {

            Permissions = permissions;
        }
        public Permission[] Permissions { get; set; } = [];
    }
}
