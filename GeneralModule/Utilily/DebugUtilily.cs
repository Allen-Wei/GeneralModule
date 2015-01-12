using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using GeneralModule.Utilily.Extension;

namespace GeneralModule.Utilily
{

    /// <summary>
    /// Debug Utilily, log path is "~/Log/"
    /// </summary>
    public class DebugUtilily
    {
        private static string _DebugPath = WebTool.svr.MapPath("~/Logs/");

        public static string SetDebugPath
        {
            set { _DebugPath = value; }
        }

        /// <summary>
        /// Write text to special file
        /// </summary>
        /// <param name="txt">write text</param>
        /// <param name="filePath">file path(absolute path with file name)</param>
        /// <returns>Writed or not</returns>
        public static bool FileDebugBase(string txt, string filePath, double maxBytes = double.NaN)
        {
            try
            {
                if (!double.IsNaN(maxBytes))
                {
                    FileInfo fInfo = new FileInfo(filePath);
                    if (fInfo.Exists)
                    {
                        if (fInfo.Length >= maxBytes)
                        {
                            fInfo.Delete();
                        }
                    }
                }
                using (StreamWriter writer = new StreamWriter(File.Open(filePath, FileMode.Append)))
                {
                    writer.WriteLine(txt);
                }
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Write text to special file
        /// </summary>
        /// <param name="txt">write text</param>
        /// <param name="fileName">file name(under ~/Logs/ path with file name)</param>
        /// <returns>Writed or not</returns>
        public static bool FileDebug(string txt, string fileName, double maxBytes = double.NaN)
        {
            string path = _DebugPath;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return FileDebugBase(txt, Path.Combine(_DebugPath, fileName), maxBytes);
        }
        /// <summary>
        /// Split File Debug
        /// </summary>
        /// <param name="txt">File content</param>
        /// <param name="filePrefixName">File prefix name</param>
        /// <param name="maxBytesPerFile">max byte per file</param>
        /// <param name="fileExtensionName">File extension name</param>
        /// <returns>true/false</returns>
        public static bool FileSplitDebug(
            string txt,
            string filePrefixName,
            double maxBytesPerFile,
            string fileExtensionName = ".split")
        {
            string fileName;
            return FileSplitDebug(txt, filePrefixName, out fileName, maxBytesPerFile, fileExtensionName);
        }

        public static bool FileSplitDebug(
            string txt,
            string filePrefixName,
            out string fileName,
            double maxBytesPerFile,
            string fileExtensionName = ".split")
        {
            var directory = new DirectoryInfo(_DebugPath);
            if (!directory.Exists)
            {
                directory.Create();
            }


            var files = directory.GetFiles(String.Format("{0} *{1}", filePrefixName, fileExtensionName));
            foreach (var aFile in files)
            {
                if (!(aFile.Length < maxBytesPerFile)) continue;

                using (var writer = aFile.AppendText())
                {
                    writer.WriteLine(txt);
                    writer.Close();
                }
                fileName = aFile.Name;
                return true;
            }

         

            fileName = String.Format("{0} {1:R}({2}){3}",
                                    filePrefixName,
                                    DateTime.Now,
                                    Guid.NewGuid(),
                                    fileExtensionName)
                                    .Replace(":", "");


            return FileDebugBase(txt, Path.Combine(_DebugPath, fileName));
        }

        public static bool FileDebug(FileDebugMode mode, string txt, string fileName = "", double maxBytes = double.NaN, string fileExtensionName = ".txt")
        {
            string directoryPath = _DebugPath;
            if (mode == FileDebugMode.Random)
            {
                return FileDebug(txt);
            }

            if (String.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }
            string filePath = directoryPath + fileName;
            if (mode == FileDebugMode.Append)
            {
                return FileDebugBase(txt, filePath);
            }
            if (mode == FileDebugMode.ReWrite)
            {
                File.WriteAllText(filePath, txt);
            }
            if (mode == FileDebugMode.Split)
            {
                return FileSplitDebug(txt, fileName, maxBytes, fileExtensionName);
            }
            return false;
        }

        /// <summary>
        /// Write text to random file(file name is random)
        /// </summary>
        /// <param name="txt">write text</param>
        /// <returns>writed or not</returns>
        public static bool FileDebug(string txt)
        {
            string fileName = Guid.NewGuid().ToString() + ".txt";
            return FileDebug(txt, fileName);
        }
        public enum FileDebugMode
        {
            Split,
            ReWrite,
            Append,
            Random
        }

    }
}