namespace project.api.Exceptions
{
    public class RefreshTokenException : ProjectException
    {
        public RefreshTokenException(
            string message,
            string sourceClass,
            string sourceMethod,
            string status) : base(message, sourceClass, sourceMethod, status)
        {
        }
    }
}
