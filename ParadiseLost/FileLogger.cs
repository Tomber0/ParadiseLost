using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParadiseLost
{
    public class FileLogger : ILogger
    {
        private object _locker = new object();
        
        private string _filePath;
        public FileLogger(string filePath) 
        {
            _filePath = filePath;

        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null) 
            {
                lock (_locker) 
                {
                    File.AppendAllText(_filePath, logLevel.ToString() +" : "+ DateTime.Now + " : " + formatter(state,exception) + Environment.NewLine);
                }
            }

        }
    }
}
