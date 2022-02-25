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
        private readonly Stack<OperationCommand> _operationCommands = new Stack<OperationCommand>();
        private readonly Stack<Command> _commands = new Stack<Command>();

        private Command? _command;

        public bool Terminated { get; private set; }

        public void Terminate()
        {
            Terminated = true;
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
                        _operationCommands.Push(operationCommand);
                    }

                    _commands.Clear();

                    _command = command;
                }

                return command.ExecuteAsync();
            }
        }

        public IAsyncEnumerable<IWritable> UndoAsync()
        {
            if (!Terminated && _operationCommands.Count > 0)
            {
                OperationCommand operationCommand = _operationCommands.Pop();

                _commands.Push(operationCommand);

                return operationCommand.UnexecuteAsync();
            }
            else
            {
                return AsyncEnumerable.Empty<IWritable>();
            }
        }

        public IAsyncEnumerable<IWritable> RedoAsync()
        {
            if (!Terminated && _commands.Count > 0)
            {
                Command command = _commands.Pop();

                if (command is OperationCommand operationCommand)
                {
                    _operationCommands.Push(operationCommand);
                }

                return command.ExecuteAsync();
            }
            else
            {
                return AsyncEnumerable.Empty<IWritable>();
            }
        }

        public IAsyncEnumerable<IWritable> ReexecuteAsync()
        {
            if (Terminated || _command is null)
            {
                return AsyncEnumerable.Empty<IWritable>();
            }
            else
            {
                return ExecuteAsync(_command);
            }
        }
    }
}
