namespace project.api.Exceptions
{
    public class DatabaseException : ProjectException
    {
        public DatabaseException(
            string message,
            string sourceClass,
            string sourceMethod,
            string status) : base(message, sourceClass, sourceMethod, status)
        {
        }
    }
}
