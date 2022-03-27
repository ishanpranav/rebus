// Ishan Pranav's REBUS: Executor.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebus
{
    /// <summary>
    /// Provides the execute, re-execute, undo, and redo functionality for <see cref="IExecutable"/> and <see cref="IUnexecutable"/> commands.
    /// </summary>
    public class Executor
    {
        private readonly Stack<IExecutable> _executables = new Stack<IExecutable>();
        private readonly Stack<IUnexecutable> _unexecutables = new Stack<IUnexecutable>();

        private IExecutable? _executable;

        /// <summary>
        /// Asynchronously executes a command and writes the output to an expression writer.
        /// </summary>
        /// <param name="executable">The executable command.</param>
        /// <param name="writer">The expression writer.</param>
        /// <returns>A task that represents the asynchronous execute operation.</returns>
        public Task ExecuteAsync(IExecutable executable, ExpressionWriter writer)
        {
            if (executable is IExecutorProvider executorProvider)
            {
                executorProvider.Executor = this;
            }
            else
            {
                if (executable is IUnexecutable unexecutable)
                {
                    _unexecutables.Push(unexecutable);
                }

                _executables.Clear();

                _executable = executable;
            }

            return executable.ExecuteAsync(writer);
        }

        /// <summary>
        /// Asynchronously re-executes the last executed command and writes the output to an expression writer.
        /// </summary>
        /// <param name="writer">The expression writer.</param>
        /// <returns>A task that represents the asynchronous re-execute operation. The task result contains a value indicating whether the operation succeeded.</returns>
        public async Task<bool> ReexecuteAsync(ExpressionWriter writer)
        {
            if (_executable is null)
            {
                return false;
            }
            else
            {
                await ExecuteAsync(_executable, writer);

                return true;
            }
        }

        /// <summary>
        /// Asynchronously undoes the last executed command and writes the output to an expression writer.
        /// </summary>
        /// <param name="writer">The expression writer.</param>
        /// <returns>A task that represents the asynchronous undo operation. The task result contains a value indicating whether the operation succeeded.</returns>
        public async Task<bool> UndoAsync(ExpressionWriter writer)
        {
            if (_unexecutables.TryPop(out IUnexecutable? result))
            {
                await result.UnexecuteAsync(writer);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Asynchronously redoes the last undone command and writes the output to an expression writer.
        /// </summary>
        /// <param name="writer">The expression writer.</param>
        /// <returns>A task that represents the asynchronous redo operation. The task result contains a value indicating whether the operation succeeded.</returns>
        public async Task<bool> RedoAsync(ExpressionWriter writer)
        {
            if (_executables.TryPop(out IExecutable? result))
            {
                if (result is IUnexecutable unexecutable)
                {
                    _unexecutables.Push(unexecutable);
                }

                await result.ExecuteAsync(writer);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
