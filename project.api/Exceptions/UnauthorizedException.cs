namespace project.api.Exceptions
{
    public class UnauthorizedException : ProjectException
    {
        public UnauthorizedException(
            string message,
            string sourceClass,
            string sourceMethod,
            string status) : base(message, sourceClass, sourceMethod, status)
        {
        }
    }
}
