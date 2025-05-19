using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystem.Services
{
    public class FileLogger : ILogger
    {
        private readonly string _logFilePath;

        public FileLogger(string logFilePath = "app.log")
        {
            _logFilePath = logFilePath;
        }

        private void WriteLog(string level, string message)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
            File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
        }

        public void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }

        public void LogError(string message)
        {
            WriteLog("ERROR", message);
        }
    }
}
