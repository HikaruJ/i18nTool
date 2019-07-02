using System;
using System.Text;

namespace i18n.Infrastructure.Entities.Contracts.IO
{
    /// <summary>
    /// Wrapper around System.IO for providing functionaliy regarding a file
    /// </summary>
    public interface IFileWrapper
    {
        /// <summary>
        /// Curreny assembly path
        /// </summary>
        /// <returns></returns>
        string AssemblyPath();

        /// <summary>
        /// Create a file in specified path using content from a string
        /// </summary>
        /// <param name="encoding">Encoding used to save the file</param>
        /// <param name="content">Content to insert into a file</param>
        /// <param name="fullPath">File path including filename</param>
        /// <returns>If created succesfully will return null, else will return the exception information</returns>
        Exception Create(Encoding encoding, string content, string fullPath);

        /// <summary>
        /// Create a file in specified path using content from a StringBuilder
        /// </summary>
        /// <param name="encoding">Encoding used to save the file</param>
        /// <param name="content">Content to insert into a file</param>
        /// <param name="fullPath">File path including filename</param>
        /// <returns>If created succesfully will return null, else will return the exception information</returns>
        Exception Create(Encoding encoding, StringBuilder content, string fullPath);

        /// <summary>
        /// Concat a path from a file name, file type and folder for an assembly path
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="fileType">File type</param>
        /// <param name="folder">File path</param>
        /// <returns>Returns a concated path from a file name, path and file type</returns>
        string CreatePathFromAssemblyPath(string fileName, string fileType, string folder);

        /// <summary>
        /// Check if a file exists in path
        /// </summary>
        /// <param name="fullPath">File path including filename</param>
        /// <returns>If found returns true, else false</returns>
        bool Exists(string fullPath);

        /// <summary>
        /// Read file content from path
        /// </summary>
        /// <param name="fullPath">File path including filename</param>
        /// <returns>If found, returns content of file, else null</returns>
        string Read(string fullPath);
    }
}
