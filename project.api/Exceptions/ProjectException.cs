using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Exceptions
{
    public class ProjectException : Exception
    {
        public ProjectError ProjectError { get; }

        public ProjectException(string message, string sourceClass, string sourceMethod, string status) : base(message)
        {
            ProjectError = new ProjectError
            {
                Type = this.GetType().Name,
                Message = message,
                SourceClass = sourceClass,
                SourceMethod = sourceMethod,
                Status = status
            };
        }
    }
}
