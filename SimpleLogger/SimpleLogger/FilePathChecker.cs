using System;
using System.IO;
using System.Linq;

namespace SimpleLogger
{
    public static class FilePathChecker
    {
        public const string FileName = "logFile.txt";
        public const string TextDir = "Text";

        public static string CheckDirectory(string rootDirectory)
        {
            var forLock = new object();

            lock (forLock)
            {
                var rootDir = new DirectoryInfo(rootDirectory);
                var now = DateTime.Now;

                var yearDir = rootDir.EnumerateDirectories().FirstOrDefault(dir => dir.Name == now.Year.ToString()) ??
                              Directory.CreateDirectory(Path.Combine(rootDirectory, now.Year.ToString()));

                var monthDir = yearDir.EnumerateDirectories().FirstOrDefault(dir => dir.Name == now.Month.ToString()) ??
                               Directory.CreateDirectory(Path.Combine(yearDir.FullName, now.Month.ToString()));

                var dayDir = monthDir.EnumerateDirectories().FirstOrDefault(dir => dir.Name == now.Day.ToString()) ??
                             Directory.CreateDirectory(Path.Combine(monthDir.FullName, now.Day.ToString()));

                var textLogDir = dayDir.EnumerateDirectories().FirstOrDefault(dir => dir.Name == TextDir) ??
                                 Directory.CreateDirectory(Path.Combine(dayDir.FullName, TextDir));

                if (!File.Exists(Path.Combine(textLogDir.FullName, FileName)))
                {
                    File.Create(Path.Combine(textLogDir.FullName, FileName));
                }

                return Path.Combine(textLogDir.FullName, FileName);
            }
        }
    }
}
