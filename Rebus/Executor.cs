// Ishan Pranav's REBUS: Executor.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus
{
    public class Executor
    {
        public int Id { get; }
        public IExecutable? Executable { get; private set; }
        public Stack<IExecutable> Executables { get; } = new Stack<IExecutable>();
        public Stack<IUnexecutable> Unexecutables { get; } = new Stack<IUnexecutable>();

        public Executor(int id)
        {
            Id = id;
        }

        public IAsyncEnumerable<IWritable> ExecuteAsync(IExecutable executable)
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

            return executable.ExecuteAsync();
        }
    }
}
