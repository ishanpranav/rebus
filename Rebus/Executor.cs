// Ishan Pranav's REBUS: Executor.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebus
{
    public class Executor
    {
        private readonly Stack<IExecutable> _executables = new Stack<IExecutable>();
        private readonly Stack<IUnexecutable> _unexecutables = new Stack<IUnexecutable>();

        private IExecutable? _executable;

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
    }
}
