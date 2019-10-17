using System;
using System.IO;

namespace Cic.OpenOne.Common.Util
{
    /// <summary>
    /// FileUtils-Klasse
    /// </summary>
    public class FileUtils
    {
        /// <summary>
        /// Saves the binary data to file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        public static void saveFile(String path, byte[] data)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            Stream byteStream = new FileStream(path, FileMode.CreateNew);
            byteStream.Write(data, 0, data.Length);
            byteStream.Close();
        }

        /// <summary>
        /// loadData
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static byte[] loadData(String file)
        {
            try
            {
                if (!File.Exists(file)) throw new Exception("File not found: " + file);
                Stream byteStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[byteStream.Length];
                byteStream.Read(buffer, 0, (int)byteStream.Length);
                byteStream.Close();
                return buffer;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// getCurrentPath
        /// </summary>
        /// <returns></returns>
        public static string getCurrentPath()
        {
            String prefix = FileUtils.getExePath(System.Reflection.Assembly.GetExecutingAssembly(), true);
            prefix = prefix.Substring(0, prefix.LastIndexOf('\\'));
            return prefix;
        }

        /// <summary>
        /// getExePath
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="addScopeName"></param>
        /// <returns></returns>
        public static string getExePath(System.Reflection.Assembly assembly, bool addScopeName)
        {
            string CodeBase;
            string ScopeName;

            // Check assembly
            if (assembly == null)
            {
                // Throw exception
                throw new Exception("assembly");
            }

            // Get code base
            CodeBase = assembly.CodeBase;
            CodeBase = CodeBase.Replace("file:///", "");
            CodeBase = CodeBase.Replace("/", @"\");

            // Check state
            if (addScopeName)
            {
                // set scope name
                ScopeName = (@"\" + assembly.ManifestModule.ScopeName);
            }
            else
            {
                // set scope name
                ScopeName = string.Empty;
            }

            try
            {
                // Return
                return (System.IO.Path.GetDirectoryName(CodeBase) + ScopeName);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }
    }
}