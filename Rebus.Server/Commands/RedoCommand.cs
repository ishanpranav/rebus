// Ishan Pranav's REBUS: RedoCommand.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Rebus.Server.Commands
{
    [Guid("5A0EE5FA-D968-49A2-AA29-8131A6B39AA9")]
    internal sealed class RedoCommand : Command, IExecutorProvider
    {
        private readonly Repository _repository;
        private readonly IStringLocalizer _localizer;

        public RedoCommand(Repository repository, IStringLocalizer localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        private RedoCommand(ArgumentSet arguments, Repository repository, IStringLocalizer localizer) : base(arguments)
        {
            _repository = repository;
            _localizer = localizer;
        }

#nullable disable
        public Executor Executor { get; set; }
#nullable enable

        public override async Task ExecuteAsync(ExpressionWriter writer)
        {
            if (await Executor.RedoAsync(writer))
            {
                writer.Write(_localizer["RedoSuccess"]);
            }
            else
            {
                writer.Write(_localizer["RedoFailure"]);
            }
        }

        public override Command CreateCommand(ArgumentSet arguments)
        {
            return new RedoCommand(arguments, _repository, _localizer);
        }
    }
}
