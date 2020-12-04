namespace project.api.Exceptions
{
    public class GuidException : ProjectException
    {
        public GuidException(
            string message,
            string sourceClass,
            string sourceMethod,
            string status) : base(message, sourceClass, sourceMethod, status)
        {
        }
    }
}
