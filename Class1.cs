using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Logger
{
    public static class Logger
    {
        public static void WriteLog(Exception ex,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            string msg = string.Empty;

            msg += ex.Message + Environment.NewLine;
            if (ex.InnerException != null)
            {
                msg += ex.InnerException + Environment.NewLine;
            }
            msg += ex.Source + Environment.NewLine;

            Log(msg, sourceFilePath, memberName, sourceLineNumber);
        }

        public static void WriteLog(string msg,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(msg, sourceFilePath, memberName, sourceLineNumber);
        }

        private static void Log(string logMessage, string sourceFileName, string methodName, int lineNum)
        {
            string currentDirectory;

            try
            {
                try
                {
                    if (Assembly.GetEntryAssembly() != null)
                    {
                        currentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    }
                    else
                    {
                        currentDirectory = System.Web.HttpContext.Current.Server.MapPath("~");
                    }
                }
                catch
                {
                    currentDirectory = string.Empty;
                }

                string filePath = ConfigurationSettings.AppSettings.AllKeys.Contains("logPath") ? ConfigurationSettings.AppSettings["logPath"] : "/log";
                if (filePath[filePath.Length - 1] != '/')
                    filePath = filePath + '/';
                if (filePath.IndexOf("/") != 0)
                    filePath = '/' + filePath;
                filePath = currentDirectory + filePath;
                CheckPath(filePath);

                string fileName = DateTime.Now.ToString("yyyyMMddHH");
                string curFile = String.Format("{0}{1}.txt", filePath, fileName);

                //write file
                StreamWriter w = File.AppendText(curFile);
                w.WriteLine("\r\n{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                w.WriteLine("\r\n{0}\r\n", logMessage);
                w.WriteLine("Method Name: {0}", methodName);
                w.WriteLine("Source File: {0}", sourceFileName);
                w.WriteLine("Line Number: {0}", lineNum);
                w.WriteLine("-------------------------------");
                w.Close();
            }
            catch
            {

            }
        }

        private static void CheckPath(string path)
        {
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            catch
            {

            }
        }

    }

}
