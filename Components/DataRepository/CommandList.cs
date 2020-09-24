using System.Collections.Generic;

namespace Components.DataRepository
{
    /// <summary>
    /// A default collection of available commands and their shortcut
    /// </summary>
    public static class CommandList
    {
        private static Dictionary<string, string> Commands = new Dictionary<string, string>
        {
            {"New file","Ctrl+N"},
            {"Open file","Ctrl+O"},
            {"Save file","Ctrl+S"},
            {"Save file as","Ctrl+Alt+S"},
            {"Close file","Ctrl+C"},
            {"Close all files","Ctrl+Alt+C"},
            {"Exit","Ctrl+Alt+X"},
            {"Undo","Ctrl+Z"},
            {"Redo","Ctrl+Y"},
            {"Cut","Ctrl+X"},
            {"Copy","Ctrl+C"},
            {"Paste","Ctrl+V"},
            {"Delete","Ctrl+D"},
            {"Select all","Ctrl+A"},
            {"Indent","Ctrl+I"},
            {"Convert case to","Ctrl+Alt+K"},
            {"Comment/Uncomment","Ctrl+K+C"},
            {"Auto-Completion","Ctrl+Alt+A"},
            {"Find","Ctrl+F"},
            {"Find in Files","Ctrl+Alt+F"},
            {"Find Next","Ctrl+Alt+N"},
            {"Find Previous","Ctrl+ALt+P"},
            {"Replace","Ctrl+R"},
            {"Go to","Ctrl+G"},
            {"Toggle full screen mode","F11"},
            {"Show symbol","Ctrl+L"},
            {"Fold all","Ctrl+Alt+H"},
            {"Unfold all","Ctrl+Alt+H"},
            {"Change Settings","F10"}
        };

        public static Dictionary<string, string> Get() => Commands;
    }
}
