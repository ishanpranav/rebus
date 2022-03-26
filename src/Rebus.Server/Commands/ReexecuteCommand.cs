// Ishan Pranav's REBUS: ReexecuteCommand.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Rebus.Server.Commands
{
    [Guid("AD7EF788-9CE6-4EA0-9A5B-7C3CFF1FB146")]
    internal sealed class ReexecuteCommand : Command, IExecutorProvider
    {
        private readonly Repository _repository;
        private readonly IStringLocalizer _localizer;

#nullable disable
        public Executor Executor { get; set; }
#nullable enable

        public ReexecuteCommand(Repository repository, IStringLocalizer localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        private ReexecuteCommand(ArgumentSet arguments, Repository repository, IStringLocalizer localizer) : base(arguments)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public override async Task ExecuteAsync(ExpressionWriter writer)
        {
            if (await Executor.ReexecuteAsync(writer))
            {
                writer.Write(_localizer["ReexecuteSuccess"]);
            }
            else
            {
                writer.Write(_localizer["ReexecuteFailure"]);
            }
        }

        public override Command CreateCommand(ArgumentSet arguments)
        {
            return new ReexecuteCommand(arguments, _repository, _localizer);
        }
    }
}
