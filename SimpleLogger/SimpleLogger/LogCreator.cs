using System;
using System.IO;
using System.Text;

namespace SimpleLogger
{
    public enum LogType
    {
        Notification = 0,
        Warning = 1,
        Error = 2,
        FatalError = 3,
        Information = 4
    }

    public class LogCreator
    {
        private static LogType _type;
        private static string _fileName;
        private static StreamWriter _fileWriter = StreamWriter.Null;

        public string RootDirectory
        {
            get
            {
                lock (_fileWriter)
                {
                    return _fileName;
                }
            }
            set
            {
                _fileName = GenerateFilePath(value);
                lock (_fileWriter)
                {
                    if (_fileWriter != StreamWriter.Null)
                    {
                        _fileWriter.Close();
                    }
                    try
                    {
                        _fileWriter = new StreamWriter(_fileName, true);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        _fileName = FilePathChecker.CheckDirectory(value);
                        _fileWriter = new StreamWriter(_fileName, true);
                    }
                    catch (FileNotFoundException)
                    {
                        _fileName = FilePathChecker.CheckDirectory(value);
                        _fileWriter = new StreamWriter(_fileName, true);
                    }
                }
            }
        }

        public void Notification(string topic, string message, bool sendEmail = false)
        {
            _type = LogType.Notification;
            CreateRecord(topic, message);
        }

        public void Warning(string topic, string message, bool sendEmail = false)
        {
            _type = LogType.Warning;
            CreateRecord(topic, message);
        }

        public void Error(string topic, string message, bool sendEmail = false)
        {
            _type = LogType.Error;
            CreateRecord(topic, message);
        }

        public void FatalError(string topic, string message, bool sendEmail = false)
        {
            _type = LogType.FatalError;
            CreateRecord(topic, message);
        }

        public void Information(string topic, string message, bool sendEmail = false)
        {
            _type = LogType.Information;
            CreateRecord(topic, message);
        }

        private void CreateRecord(string topic, string message)
        {
            var now = DateTime.Now;
            var builder = new StringBuilder();
            builder.AppendFormat("{0}/{1}/{2} {3}:{4}:{5} {6}: topic {7}; {8}",
                now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Second, _type, topic, message);
            lock (_fileWriter)
            {
                _fileWriter.WriteLine(builder.ToString());
                ReopenWriter();
            }
        }

        private static void ReopenWriter()
        {
            lock (_fileWriter)
            {
                _fileWriter.Close();
                _fileWriter = new StreamWriter(_fileName, true);
            }
        }

        private static string GenerateFilePath(string rootDir)
        {
            var now = DateTime.Now;
            return Path.Combine(rootDir, now.Year.ToString(), now.Month.ToString(), now.Day.ToString(),
                FilePathChecker.TextDir, FilePathChecker.FileName);
        }
    }
}
