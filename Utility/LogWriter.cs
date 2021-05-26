using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace buildxact_supplies.Utility
{
    public static class LogWriter
    {
        private static string LogPath;

        static LogWriter()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();
            LogPath = config["LogPath"];
        }

        public static void LogWrite(string logMessage)
        {
            try
            {
                using (StreamWriter w = File.AppendText(LogPath))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private static void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                txtWriter.WriteLine(":{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
