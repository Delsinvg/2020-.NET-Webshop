﻿namespace project.api.Exceptions
{
    public class IdentityException : ProjectException
    {
        public IdentityException(
            string message,
            string sourceClass,
            string sourceMethod,
            string status) : base(message, sourceClass, sourceMethod, status)
        {
        }
    }
}
