using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Exceptions
{
    public class ProjectError
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string SourceClass { get; set; }
        public string SourceMethod { get; set; }
        public string Status { get; set; }
    }
}
