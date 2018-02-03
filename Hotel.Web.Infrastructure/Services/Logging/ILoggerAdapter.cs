namespace Hotel.Web.Infrastructure.Services.Logging
{
    public interface ILoggerAdapter<T>
    {
        // add just the logger method(s) your app uses
        void LogError(string message, params object[] args);
    }
}
