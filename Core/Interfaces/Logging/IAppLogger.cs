﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Logging
{
    public interface IAppLogger
    {
        void Debug(string message);
        void Information(string message);
        void Warning(string message);
        void Error(string message, Exception ex = null);
        void Critical(string message, Exception ex = null);
    }

}
