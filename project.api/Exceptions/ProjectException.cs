﻿using System;
using System.Text.Json.Serialization;

namespace project.api.Exceptions
{

    [JsonConverter(typeof(ProjectExceptionConverter))]
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
        public ProjectException(string type, string message, string sourceClass, string sourceMethod, string status) : base(message)
        {
            ProjectError = new ProjectError
            {
                Type = type,
                Message = message,
                SourceClass = sourceClass,
                SourceMethod = sourceMethod,
                Status = status
            };
        }
    }
}
