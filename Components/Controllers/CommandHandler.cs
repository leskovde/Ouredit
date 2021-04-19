using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Components.Commands;

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
        private readonly Stack<ICommand> _executeCommandHistory = new Stack<ICommand>();
        private readonly Stack<ICommand> _undoCommandHistory = new Stack<ICommand>();

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
            return _executeCommandHistory.Select(x => x.Name).ToList();
        }

        /// <summary>
        /// Executes a single command and adds it to the command history. Clears the undo history since all undoing has been done, the user now invokes commands.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command object whose execute methods should be called.</param>
        public async Task Execute<T>(T command) where T : ICommand
        {
            var task = new Task(() =>
            {
                command.Execute();
                _executeCommandHistory.Push(command);
                _undoCommandHistory.Clear();
                AutoSaver.Instance.Trigger();
            });
            
            task.Start();
            await task;
        }

        /// <summary>
        /// Undoes a given number of commands (if that number is valid) and removes them from the command history.
        /// Adds the command to the undo history for the possible redo.
        /// </summary>
        /// <param name="numberOfCommands">The number of commands to be undone. Default is one.</param>
        public async Task Undo(int numberOfCommands = 1)
        {
            var task = new Task(() =>
            {
                if (numberOfCommands < 1 || numberOfCommands > _executeCommandHistory.Count)
                {
                    throw new InvalidOperationException();
                }

                for (var i = 0; i < numberOfCommands; i++)
                {
                    var command = _executeCommandHistory.Pop();
                    command.Undo();
                    _undoCommandHistory.Push(command);
                }

                AutoSaver.Instance.Trigger();
            });

            task.Start();
            await task;
        }

        /// <summary>
        /// Redoes a given number of undone commands (if that number is valid) and removes them from the undo command history.
        /// </summary>
        /// <param name="numberOfCommands">The number of commands to be redone. Default is one.</param>
        public async Task Redo(int numberOfCommands = 1)
        {
            var task = new Task(() =>
            {
                if (numberOfCommands < 1 || numberOfCommands > _undoCommandHistory.Count)
                {
                    throw new InvalidOperationException();
                }

                for (var i = 0; i < numberOfCommands; i++)
                {
                    var command = _undoCommandHistory.Pop();
                    command.Execute();
                }

                AutoSaver.Instance.Trigger();
            });

            task.Start();
            await task;
        }
    }
}
