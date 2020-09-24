using System;
using System.Collections.Generic;
using System.Text;
using Components.Models;

namespace Components.Controllers
{
    [Leskovar]
    public class AutoSaver
    {
        private static AutoSaver _instance;
        private static readonly object Mutex = new object();
        private string _currentFilePath;

        private AutoSaver()
        {
        }

        /// <summary>
        /// Makes sure only one instance is ever created.
        /// </summary>
        /// <returns>Reference to the singleton instance.</returns>
        public static AutoSaver Instance
        {
            get
            {
                lock (Mutex)
                {
                    return _instance ??= new AutoSaver();
                }
            }
        }

        public void Update(string filePath)
        {
            _currentFilePath = filePath;
        }

        public void Trigger()
        {
            Console.WriteLine($"#DEBUG: Saving the file {_currentFilePath}.");
            ApplicationState.Instance.FileHandlerInstance.SaveFile(_currentFilePath, _currentFilePath);
        }
    }
}
