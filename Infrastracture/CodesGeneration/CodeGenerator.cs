using Infrastracture.Logic.CodesGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.CodesGeneration
{
    public class CodeGenerator : ICodeGenerator
    {
        public int GenerateSecureCode()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] data = new byte[4];
                rng.GetBytes(data);
                int value = BitConverter.ToInt32(data, 0) & 0x7FFFFFFF;
                return value % 90000 + 10000;
            }
        }
    }
}
