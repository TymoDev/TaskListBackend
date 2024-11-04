using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.UserDTO.Request
{
    public record UserRequestId(Guid id,string Username, string Email, string Password);
}
