using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components;
using Microsoft.Extensions.Configuration;

namespace OurTextEditor
{
    public delegate void CurrentFileChangeDelegate(object sender, CurrentFileChangeArgs args);

    /// <summary>
    /// Hold the CurrentFileChange arguments, i.e. the updated path to the current file.
    /// </summary>
    [Leskovar]
    public class CurrentFileChangeArgs : EventArgs
    {
        public string FilePath { get; }

        public CurrentFileChangeArgs(string filePath)
        {
            FilePath = filePath;
        }
    }

    /// <summary>
    /// Provides the contract for the CurrentFileChange service.
    /// </summary>
    [Leskovar]
    public interface ICurrentFileChangeBroadcastService
    {
        event CurrentFileChangeDelegate OnCurrentFileChanged;
        string GetCurrentValue();
    }

    /// <summary>
    /// Serves as the middle man between the MenuActions.CurrentFileChanged notification event and the razor component that needs to be updated.
    /// </summary>
    [Leskovar]
    public class CurrentFileChangeBroadcastService : ICurrentFileChangeBroadcastService
    {
        public event CurrentFileChangeDelegate OnCurrentFileChanged;
        private IConfiguration _configuration;

        public CurrentFileChangeBroadcastService(IConfiguration configuration)
        {
            _configuration = configuration;
            MenuActions.CurrentFileChanged += CurrentFile_Changed;
        }

        /// <summary>
        /// Gathers argument data and redirects the notification event further.
        /// </summary>
        /// <param name="sender">The sender object of the notification event.</param>
        /// <param name="e">The notification event argument. Currently empty and unused.</param>
        private void CurrentFile_Changed(object sender, EventArgs e)
        {
            Console.WriteLine("#DEBUG: The notification has been received by the CurrentFileChangedBroadcastService.");
            OnCurrentFileChanged?.Invoke(this, new CurrentFileChangeArgs(MenuActions.CurrentFilePath));
        }

        /// <summary>
        /// Returns the current data when no event has been called yet.
        /// </summary>
        /// <returns>The path to the current file.</returns>
        public string GetCurrentValue()
        {
            return MenuActions.CurrentFilePath;
        }
    }
}
