using AIWolf.Common.Util;
using System;
using System.IO;

namespace AIWolf.Server.Util
{
    /// <summary>
    /// GameLogger using file.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    class FileGameLogger : IGameLogger
    {
        string logFile;
        TextWriter writer;

        public FileGameLogger(string path)
        {
            if (path != null)
            {
                if (Directory.Exists(path))
                {
                    logFile = Path.ChangeExtension(Path.Combine(path, CalendarTools.ToTimeString(DateTime.Now)), ".log");
                }
                else
                {
                    logFile = path;
                }
                Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(logFile)));
                try
                {
                    writer = new StreamWriter(logFile);
                    return;
                }
                catch (IOException e)
                {
                    Console.Error.WriteLine(e.StackTrace);
                    Console.Error.WriteLine("Fail to create logfile. Output log to Console.Out");
                }
            }
            writer = Console.Out;
        }

        public void Close()
        {
            writer.Close();
        }

        public void Flush()
        {
            writer.Flush();
        }

        /// <summary>
        /// Save log.
        /// </summary>
        /// <param name="log"></param>
        public void Log(string log)
        {
            writer.WriteLine(log);
        }
    }
}
