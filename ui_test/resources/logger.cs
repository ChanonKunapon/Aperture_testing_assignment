using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ui_tests.resources
{
    public static class Logger
    {
        // Set the dynamic base path
        private static readonly string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        // Generate a unique log file name based on the current date and time
        private static readonly string logFileName = $"ui_test_result_{DateTime.Now:yyyyMMdd_HHmmssfff}.txt";

        // Log file path
        private static readonly string logFilePath = Path.Combine(baseDirectory, @"result_ui_test_log", logFileName);

        // Initialize the logger
        static Logger()
        {
            try
            {
                // Resolve the absolute path for the log file
                string absoluteLogPath = Path.GetFullPath(logFilePath);

                // Print the resolved path for debugging
                Console.WriteLine($"Log file path: {absoluteLogPath}");

                // Create the log directory if it doesn't exist
                string directory = Path.GetDirectoryName(absoluteLogPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Create the log file at the start of the test run
                using (FileStream fs = File.Create(absoluteLogPath))
                {
                    // Ensure file is created
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to initialize logger. Path: {logFilePath}, Error: {ex.Message}");
            }
        }

        // Log a message to the console and also write it to the log file
        public static void Log(string message)
        {
            try
            {
                // Write to Console
                Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");

                // Write to log file
                string absoluteLogPath = Path.GetFullPath(logFilePath);
                using (StreamWriter sw = new StreamWriter(absoluteLogPath, true))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to log message. Path: {logFilePath}, Error: {ex.Message}");
            }
        }
    }
}
