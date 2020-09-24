using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components;
using Components.Models;
using Microsoft.Extensions.Configuration;

namespace OurTextEditor
{
    public delegate void OpenFilesChangeDelegate(object sender, OpenFilesChangeArgs args);

    /// <summary>
    /// Hold the OpenFilesChange arguments, i.e. the updated list of open file names.
    /// </summary>
    [Leskovar]
    public class OpenFilesChangeArgs : EventArgs
    {
        public List<string> OpenFiles { get; }

        public OpenFilesChangeArgs(List<string> openFiles)
        {
            OpenFiles = openFiles;
        }
    }

    /// <summary>
    /// Provides the contract for the OpenFilesChange service.
    /// </summary>
    [Leskovar]
    public interface IOpenFilesChangeBroadcastService
    {
        event OpenFilesChangeDelegate OnOpenFilesChanged;
        List<string> GetCurrentValue();
    }

    /// <summary>
    /// Serves as the middle man between the MenuActions.OpenFilesChanged notification event and the razor component that needs to be updated.
    /// </summary>
    [Leskovar]
    public class OpenFilesChangeBroadcastService : IOpenFilesChangeBroadcastService
    {
        public event OpenFilesChangeDelegate OnOpenFilesChanged;
        private IConfiguration _configuration;

        public OpenFilesChangeBroadcastService(IConfiguration configuration)
        {
            _configuration = configuration;
            MenuActions.OpenFilesChanged += OpenFiles_Changed;
        }

        /// <summary>
        /// Gathers argument data and redirects the notification event further.
        /// </summary>
        /// <param name="sender">The sender object of the notification event.</param>
        /// <param name="e">The notification event argument. Currently empty and unused.</param>
        private void OpenFiles_Changed(object sender, EventArgs e)
        {
            Console.WriteLine("#DEBUG: The notification has been received by the OpenFilesChangedBroadcastService.");
            OnOpenFilesChanged?.Invoke(this, new OpenFilesChangeArgs(ApplicationState.Instance.FileHandlerInstance.GetOpenFilePaths()));
        }

        /// <summary>
        /// Returns the current data when no event has been called yet.
        /// </summary>
        /// <returns>The updated list of open file paths.</returns>
        public List<string> GetCurrentValue()
        {
            return ApplicationState.Instance.FileHandlerInstance.GetOpenFilePaths();
        }
    }
}
