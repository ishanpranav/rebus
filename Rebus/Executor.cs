// Ishan Pranav's REBUS: Executor.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using Rebus.Commands;

namespace Rebus
{
    public class Executor
    {
        internal Command? Command { get; private set; }
        internal Stack<Command> Commands { get; } = new Stack<Command>();
        internal Stack<OperationCommand> OperationCommands { get; } = new Stack<OperationCommand>();

        public int Id { get; }
        public bool Terminated { get; internal set; }

        public Executor(int id)
        {
            Id = id;
        }

        public IAsyncEnumerable<IWritable> ExecuteAsync(Command command)
        {
            if (Terminated)
            {
                return AsyncEnumerable.Empty<IWritable>();
            }
            else
            {
                if (command is SystemCommand systemCommand)
                {
                    systemCommand.Executor = this;
                }
                else
                {
                    if (command is OperationCommand operationCommand)
                    {
                        OperationCommands.Push(operationCommand);
                    }

                    Commands.Clear();

                    Command = command;
                }

                return command.ExecuteAsync();
            }
        }
    }
}
