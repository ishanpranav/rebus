// Ishan Pranav's REBUS: Executor.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Rebus.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebus
{
    public class Executor
    {
        private readonly Stack<OperationCommand> _operationCommands = new Stack<OperationCommand>();
        private readonly Stack<Command> _commands = new Stack<Command>();

        private Command? _command;

        public Task<IWritable> ExecuteAsync(Command command)
        {
            if (command is SystemCommand systemCommand)
            {
                systemCommand.Executor = this;
            }
            else
            {
                if (command is OperationCommand operationCommand)
                {
                    this._operationCommands.Push(operationCommand);
                }

                this._commands.Clear();

                this._command = command;
            }

            return command.ExecuteAsync();
        }

        public async Task<bool> UndoAsync()
        {
            if (this._operationCommands.Count > 0)
            {
                OperationCommand operationCommand = this._operationCommands.Pop();

                await operationCommand.UnexecuteAsync();

                this._commands.Push(operationCommand);

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RedoAsync()
        {
            if (this._commands.Count > 0)
            {
                Command command = this._commands.Pop();

                await command.ExecuteAsync();

                if (command is OperationCommand operationCommand)
                {
                    this._operationCommands.Push(operationCommand);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ReexecuteAsync()
        {
            if (this._command is null)
            {
                return false;
            }
            else
            {
                await this.ExecuteAsync(this._command);

                return true;
            }
        }
    }
}