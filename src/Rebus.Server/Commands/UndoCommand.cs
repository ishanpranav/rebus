// Ishan Pranav's REBUS: UndoCommand.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Rebus.Server.Commands
{
    [Guid("86EEE5CE-5FFF-4420-9B3F-5A4DC96B54A0")]
    internal sealed class UndoCommand : Command, IExecutorProvider
    {
        private readonly DbRepository _repository;

#nullable disable
        public Executor Executor { get; set; }
#nullable enable

        public UndoCommand(DbRepository repository)
        {
            _repository = repository;
        }

        private UndoCommand(ArgumentSet arguments, DbRepository repository) : base(arguments)
        {
            _repository = repository;
        }

        public override async Task ExecuteAsync(ExpressionWriter writer)
        {
            if (await Executor.UndoAsync(writer))
            {
                writer.Write(_repository["UndoSuccess"]);
            }
            else
            {
                writer.Write(_repository["UndoFailure"]);
            }
        }

        public override Command CreateCommand(ArgumentSet arguments)
        {
            return new UndoCommand(arguments, _repository);
        }
    }
}
