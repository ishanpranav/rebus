// Ishan Pranav's REBUS: RedoCommand.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Rebus.Server.Commands
{
    [Guid("5A0EE5FA-D968-49A2-AA29-8131A6B39AA9")]
    internal sealed class RedoCommand : Command, IExecutorProvider
    {
        private readonly DbRepository _repository;

        public RedoCommand(DbRepository repository)
        {
            _repository = repository;
        }

        private RedoCommand(ArgumentSet arguments, DbRepository repository) : base(arguments)
        {
            _repository = repository;
        }

#nullable disable
        public Executor Executor { get; set; }
#nullable enable

        public override async Task ExecuteAsync(ExpressionWriter writer)
        {
            if (await Executor.RedoAsync(writer))
            {
                writer.Write(_repository["RedoSuccess"]);
            }
            else
            {
                writer.Write(_repository["RedoFailure"]);
            }
        }

        public override Command CreateCommand(ArgumentSet arguments)
        {
            return new RedoCommand(arguments, _repository);
        }
    }
}
