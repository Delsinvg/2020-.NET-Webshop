using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Exceptions
{
    public class PasswordException : ProjectException
    {
        public PasswordException(
            string message,
            string sourceClass,
            string sourceMethod,
            string status) : base(message, sourceClass, sourceMethod, status)
        {
        }
    }
}
