namespace project.api.Exceptions
{
    public class EntityException : ProjectException
    {
        public EntityException(
            string message,
            string sourceClass,
            string sourceMethod,
            string status) : base(message, sourceClass, sourceMethod, status)
        {
        }
    }
}
