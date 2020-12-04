namespace project.api.Exceptions
{
    public class UserNameException : ProjectException
    {
        public UserNameException(
            string message,
            string sourceClass,
            string sourceMethod,
            string status) : base(message, sourceClass, sourceMethod, status)
        {
        }
    }
}
