using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Components.Controllers
{
    /// <summary>
    /// A singleton command handler that keeps note of used commands in a stack. Call CommandInvoker.Instance to get the object reference.
    /// </summary>
    [Leskovar]
    public class CommandInvoker
    {
        private static CommandInvoker _instance;
        private static readonly object Mutex = new object();
        private readonly Stack<ICommand> _commandHistory = new Stack<ICommand>();

        private CommandInvoker()
        {
        }

        /// <summary>
        /// Makes sure only one instance is ever created.
        /// </summary>
        /// <returns>Reference to the singleton instance.</returns>
        public static CommandInvoker Instance
        {
            get
            {
                lock (Mutex)
                {
                    return _instance ??= new CommandInvoker();
                }
            }
        }

        /// <summary>
        /// Looks up the command stack to get the command history.
        /// </summary>
        /// <returns>A list of command names in an ordered manner - recent commands come first.</returns>
        public List<string> GetCommandHistory()
        {
            return _commandHistory.Select(x => x.Name).ToList();
        }

        /// <summary>
        /// Executes a single command and adds it to the command history.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command object whose execute methods should be called.</param>
        public void Execute<T>(T command) where T : ICommand
        {
            command.Execute();
            _commandHistory.Push(command);
        }

        /// <summary>
        /// Undoes a given number of commands (if that number is valid) and removes them from the command history.
        /// </summary>
        /// <param name="numberOfCommands">The number of commands to be undone. Default is one.</param>
        public void Undo(int numberOfCommands = 1)
        {
            if (numberOfCommands < 1 || numberOfCommands > _commandHistory.Count)
            {
                throw new InvalidOperationException();
            }

            for (var i = 0; i < numberOfCommands; i++)
            {
                var command = _commandHistory.Pop();
                command.Undo();
            }
        }
    }
}
