// Ishan Pranav's REBUS: ReexecuteCommand.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Rebus.Server.Commands
{
    [Guid("AD7EF788-9CE6-4EA0-9A5B-7C3CFF1FB146")]
    internal sealed class ReexecuteCommand : Command, IExecutorProvider
    {
        private readonly DbRepository _repository;

#nullable disable
        public Executor Executor { get; set; }
#nullable enable

        public ReexecuteCommand(DbRepository repository)
        {
            _repository = repository;
        }

        private ReexecuteCommand(ArgumentSet arguments, DbRepository repository) : base(arguments)
        {
            _repository = repository;
        }

        public override async Task ExecuteAsync(ExpressionWriter writer)
        {
            if (await Executor.ReexecuteAsync(writer))
            {
                writer.Write(_repository["ReexecuteSuccess"]);
            }
            else
            {
                writer.Write(_repository["ReexecuteFailure"]);
            }
        }

        public override Command CreateCommand(ArgumentSet arguments)
        {
            return new ReexecuteCommand(arguments, _repository);
        }
    }
}
