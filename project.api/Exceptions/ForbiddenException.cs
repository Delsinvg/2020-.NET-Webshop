using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Exceptions
{
    public class ForbiddenException : ProjectException
    {
        public ForbiddenException(
            string message,
            string sourceClass,
            string sourceMethod,
            string status) : base(message, sourceClass, sourceMethod, status)
        {
        }
    }
}
