﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.UserDTO.Request
{
    public record ResetPasswordVerifyRequest(string Email, int Code);
}
