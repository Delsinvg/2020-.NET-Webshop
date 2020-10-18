using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Exceptions
{
    public class IdentityException : Exception
    {
        public string Title { get; }
        public string SourceClass { get; }
        public string SourceMethod { get; }

        public IdentityException(string message, string sourceClass, string sourceMethod) : base(message)
        {
            Title = "IdentityException";
            SourceClass = sourceClass;
            SourceMethod = sourceMethod;
        }
    }
}
