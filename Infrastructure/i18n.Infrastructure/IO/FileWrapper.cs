using i18n.Infrastructure.Entities.Contracts.IO;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace i18n.Infrastructure.IO
{
    public class FileWrapper : IFileWrapper
    {
        #region Private Readonly Members

        private readonly ILogger<FileWrapper> _logger = null;

        #endregion

        #region CTOR

        public FileWrapper(ILogger<FileWrapper> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Curreny assembly path
        /// </summary>
        /// <returns></returns>
        public string AssemblyPath()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        /// <summary>
        /// Create a file in specified path using content from a string
        /// </summary>
        /// <param name="encoding">Encoding used to save the file</param>
        /// <param name="content">Content to insert into a file</param>
        /// <param name="fullPath">File path including filename</param>
        /// <returns>If created succesfully will return null, else will return the exception information</returns>
        public Exception Create(Encoding encoding, string content, string fullPath)
        {
            try
            {
                File.WriteAllText(fullPath, content.ToString(), encoding);
                return null;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to create file in path '{fullPath}'");
                return ex;
            }
        }

        /// <summary>
        /// Create a file in specified path using content from a StringBuilder
        /// </summary>
        /// <param name="encoding">Encoding used to save the file</param>
        /// <param name="content">Content to insert into a file</param>
        /// <param name="fullPath">File path including filename</param>
        /// <returns>If created succesfully will return null, else will return the exception information</returns>
        public Exception Create(Encoding encoding, StringBuilder content, string fullPath)
        {
            try
            {
                File.WriteAllText(fullPath, content.ToString(), encoding);
                return null;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to create file in path '{fullPath}'");
                return ex;
            }
        }

        /// <summary>
        /// Concat a path from a file name, file type and folder for an assembly path
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="fileType">File type</param>
        /// <param name="folder">File path</param>
        /// <returns>Returns a concated path from a file name, path and file type</returns>
        public string CreatePathFromAssemblyPath(string fileName, string fileType, string folder)
        {
            return $"{AssemblyPath()}\\{folder}\\{fileName}.{fileType}";
        }

        /// <summary>
        /// Check if a file exists in path
        /// </summary>
        /// <param name="fullPath">File path including filename</param>
        /// <returns>If found returns true, else false</returns>
        public bool Exists(string fullPath)
        {
            return File.Exists(fullPath);
        }

        /// <summary>
        /// Read file content from path
        /// </summary>
        /// <param name="fullPath">File path including filename</param>
        /// <returns>If found, returns content of file, else null</returns>
        public string Read(string fullPath)
        {
            string reader = null;

            try
            {
                reader = File.ReadAllText(fullPath);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to read file in path '{fullPath}'");
            }

            return reader;
        }

        #endregion
    }
}
