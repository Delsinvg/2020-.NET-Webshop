using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Exceptions
{
    public class CollectionException : Exception
    {
        public string Type { get; }
        public string Title { get; }
        public string SourceClass { get; }
        public string SourceMethod { get; }

        public CollectionException(string message, string sourceClass, string sourceMethod) : base(message)
        {
            Type = "CollectionException";
            Title = message;
            SourceClass = sourceClass;
            SourceMethod = sourceMethod;
        }
    }
}
