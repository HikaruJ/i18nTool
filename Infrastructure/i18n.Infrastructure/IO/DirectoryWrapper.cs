using i18n.Infrastructure.Entities.Contracts.IO;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace i18n.Infrastructure.IO
{
    /// <summary>
    /// Wrapper around System.IO for providing functionaliy regarding a directory
    /// </summary>
    public class DirectoryWrapper : IDirectoryWrapper
    {
        #region Private Readonly Members

        private readonly ILogger<DirectoryWrapper> _logger = null;

        #endregion

        #region CTOR

        public DirectoryWrapper(ILogger<DirectoryWrapper> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Public Methods 

        /// <summary>
        /// Create a directory in a specified path (if the directory does not exists)
        /// </summary>
        /// <param name="path">Folder path</param>
        /// <returns>If created succesfully will return null, else will return the exception information</returns>
        public Exception Create(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    // Try to create the directory.
                    DirectoryInfo directory = Directory.CreateDirectory(path);
                }

                return null;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Faild to create directory in path '{path}'");
                return ex;
            }
        }

        #endregion
    }
}
