// Ishan Pranav's REBUS: Executor.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebus
{
    public class Executor
    {
        public IExecutable? Executable { get; private set; }
        public Stack<IExecutable> Executables { get; } = new Stack<IExecutable>();
        public Stack<IUnexecutable> Unexecutables { get; } = new Stack<IUnexecutable>();

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
                    Unexecutables.Push(unexecutable);
                }

                Executables.Clear();

                Executable = executable;
            }

            return executable.ExecuteAsync(writer);
        }
    }
}
