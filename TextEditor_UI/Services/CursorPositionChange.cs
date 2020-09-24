using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components;
using Components.Models;
using Microsoft.Extensions.Configuration;
using OurTextEditor.Component;

namespace OurTextEditor
{
    public delegate void CursorPositionChangeDelegate(object sender, CursorPositionChangeArgs args);

    /// <summary>
    /// Hold the CursorPositionChange arguments, i.e. the number of character from the start.
    /// </summary>
    [Leskovar]
    public class CursorPositionChangeArgs : EventArgs
    {
        public int CursorPosition { get; }

        public CursorPositionChangeArgs(int cursorPosition)
        {
            CursorPosition = cursorPosition;
        }
    }

    /// <summary>
    /// Provides the contract for the CursorPositionChange service.
    /// </summary>
    [Leskovar]
    public interface ICursorPositionChangeBroadcastService
    {
        event CursorPositionChangeDelegate OnCursorPositionChanged;
        int GetCurrentValue();
    }

    /// <summary>
    /// Serves as the middle man between the FileContent.CursorPositionChanged notification event and the buffer that needs to be updated.
    /// </summary>
    [Leskovar]
    public class CursorPositionChangeBroadcastService : ICursorPositionChangeBroadcastService
    {
        public event CursorPositionChangeDelegate OnCursorPositionChanged;
        private IConfiguration _configuration;

        public CursorPositionChangeBroadcastService(IConfiguration configuration)
        {
            _configuration = configuration;
            FileContent.CursorPositionChanged += CursorPosition_Changed;
        }

        /// <summary>
        /// Gathers argument data and redirects the notification event further.
        /// </summary>
        /// <param name="sender">The sender object of the notification event.</param>
        /// <param name="e">The notification event argument, i.e. the number of characters from the start to the cursor</param>
        private void CursorPosition_Changed(object sender, CursorPositionChangeArgs e)
        {
            Console.WriteLine("#DEBUG: The notification has been received by the CursorPositionChangeBroadcastService.");
            OnCursorPositionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Returns the current data when no event has been called yet.
        /// </summary>
        /// <returns>Zero.</returns>
        public int GetCurrentValue()
        {
            return 0;
        }
    }
}
