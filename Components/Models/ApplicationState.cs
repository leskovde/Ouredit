using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Components.Models
{
    /// <summary>
    /// Stores the current state of the text editor.
    /// </summary>
    public class ApplicationState
    {
        private List<File> _files;

        public ApplicationState(List<string> filePaths)
        {
            _files = new List<File>();

            foreach (var filePath in filePaths)
            {
                _files.Add(new File(filePath));
            }
        }
    }
}
