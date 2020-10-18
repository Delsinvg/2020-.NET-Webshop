using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Exceptions
{
    public class DatabaseException : Exception
    {
        public string Type { get; }
        public string Title { get; }
        public string SourceClass { get; }
        public string SourceMethod { get; }

        public DatabaseException(string type, string message, string sourceClass, string sourceMethod) : base(message)
        {
            Type = type;
            Title = message;
            SourceClass = sourceClass;
            SourceMethod = sourceMethod;
        }
    }
}
