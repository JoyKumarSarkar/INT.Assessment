using INT.Assessment.ENTITY;
using Microsoft.Extensions.Configuration;

namespace INT.Assessment.LOGGER
{
    public class FileLogger : IFileLogger
    {
        private readonly IConfiguration _Configuration;
        private readonly object _Lock = new();

        public FileLogger (IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }

        private void WriteLog (LogLevel Level, string Message, LogAssembly LogAssembly, Exception Ex = null)
        {
            string LogDirectory = _Configuration["LoggingSettings:LogDirectory"]!;
            string ApplicationName = _Configuration["Application:Name"]!;

            if (!Directory.Exists(LogDirectory))
                Directory.CreateDirectory(LogDirectory);

            var LogFile = Path.Combine(LogDirectory, $"{ApplicationName}.LOG-{DateTime.Now:dd-MMM-yyyy}.txt");

            var LogEntry = $"{DateTime.Now:dd-MMM-yyyy HH:mm:ss.fff} [{LogAssembly.ToString()}] [{Level.ToString()}] {Message}";

            if (Ex != null)
                LogEntry += $" | Exception: {Ex}";

            lock (_Lock)
            {
                File.AppendAllText(LogFile, LogEntry + Environment.NewLine);
            }
        }

        public void Info (string Message, LogAssembly Assembly) => WriteLog (LogLevel.INFO, Message, Assembly);
        public void Warning (string Message, LogAssembly Assembly) => WriteLog (LogLevel.WARNING, Message, Assembly);
        public void Error (string Message, LogAssembly Assembly, Exception Ex = null) => WriteLog (LogLevel.ERROR, Message, Assembly, Ex);
    }

    
}
