// Ishan Pranav's REBUS: UndoCommand.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Rebus.Server.Commands
{
    [Guid("86EEE5CE-5FFF-4420-9B3F-5A4DC96B54A0")]
    internal sealed class UndoCommand : Command, IExecutorProvider
    {
        private readonly Repository _repository;
        private readonly IStringLocalizer _localizer;

#nullable disable
        public Executor Executor { get; set; }
#nullable enable

        public UndoCommand(Repository repository, IStringLocalizer localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        private UndoCommand(ArgumentSet arguments, Repository repository, IStringLocalizer localizer) : base(arguments)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public override async Task ExecuteAsync(ExpressionWriter writer)
        {
            if (await Executor.UndoAsync(writer))
            {
                writer.Write(_localizer["UndoSuccess"]);
            }
            else
            {
                writer.Write(_localizer["UndoFailure"]);
            }
        }

        public override Command CreateCommand(ArgumentSet arguments)
        {
            return new UndoCommand(arguments, _repository, _localizer);
        }
    }
}
