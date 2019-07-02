using System;

namespace i18n.Infrastructure.Entities.Contracts.IO
{
    /// <summary>
    /// Wrapper around System.IO for providing functionaliy regarding a directory
    /// </summary>
    public interface IDirectoryWrapper
    {
        /// <summary>
        /// Create a directory in a specified path (if the directory does not exists)
        /// </summary>
        /// <param name="path">Folder path</param>
        /// <returns>If created succesfully will return null, else will return the exception information</returns>
        Exception Create(string path);
    }
}
