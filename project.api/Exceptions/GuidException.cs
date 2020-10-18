using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Exceptions
{
    public class GuidException : Exception
    {
        public string Type { get; }
        public string Title { get; }
        public string SourceClass { get; }
        public string SourceMethod { get; }

        public GuidException(string message, string sourceClass, string sourceMethod) : base(message)
        {
            Type = "GuidException";
            Title = message;
            SourceClass = sourceClass;
            SourceMethod = sourceMethod;
        }
    }
}
