// Ishan Pranav's REBUS: Executor.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;
using Rebus.Commands;

namespace Rebus
{
    public class Executor
    {
        private readonly Stack<OperationCommand> _operationCommands = new Stack<OperationCommand>();
        private readonly Stack<Command> _commands = new Stack<Command>();

        private Command? _command;

        public Task<IWritable?> ExecuteAsync(Command command)
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

        public Task<IWritable?> UndoAsync()
        {
            if (_operationCommands.Count > 0)
            {
                OperationCommand operationCommand = _operationCommands.Pop();

                _commands.Push(operationCommand);

                return operationCommand.UnexecuteAsync();
            }
            else
            {
                return Task.FromResult<IWritable?>(null);
            }
        }

        public Task<IWritable?> RedoAsync()
        {
            if (_commands.Count > 0)
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
                return Task.FromResult<IWritable?>(null);
            }
        }

        public Task<IWritable?> ReexecuteAsync()
        {
            if (_command is null)
            {
                return Task.FromResult<IWritable?>(null);
            }
            else
            {
                return ExecuteAsync(_command);
            }
        }
    }
}