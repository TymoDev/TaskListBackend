using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.UserDTO.Responce
{
    public record UserResponce(Guid Id, string Username, string Email);
}
