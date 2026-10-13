namespace ClinicManagement.Model.Helper
{
    public class DbExceptionHelper : Exception
    {
        public DbExceptionHelper()
        {
        }

        public DbExceptionHelper(string message) : base("In Db: " + message)
        {
        }

        public DbExceptionHelper(string message, Exception innerException) : base("In Db: " + message, innerException)
        {
        }
    }
}
