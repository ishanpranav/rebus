// Ishan Pranav's REBUS: RenameCommand.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Rebus.Server.Concepts;

namespace Rebus.Server.Commands
{
    [Guid("45DA897D-68E8-47D5-A946-76FDE16B826B")]
    internal sealed class RenameCommand : Command
    {
        private readonly DbRepository _repository;

        public RenameCommand(DbRepository repository)
        {
            _repository = repository;
        }

        private RenameCommand(ArgumentSet arguments, DbRepository repository) : base(arguments)
        {
            _repository = repository;
        }

        public override bool Matches(ArgumentSet arguments)
        {
            return arguments.IsPlayer(Argument.Subject) && arguments.IsQuotation(Argument.DirectObject) && arguments.TryGetConcepts(Argument.IndirectObject, out IReadOnlyCollection<Concept>? indirectObjects) && indirectObjects.Count == 1;
        }

        public override async Task ExecuteAsync(ExpressionWriter writer)
        {
            Concept concept = Arguments
                .GetConcepts(Argument.IndirectObject)
                .OfType<Spacecraft>()
                .First(x => x.PlayerId == Arguments.PlayerId);

            concept.Write(writer);

            await _repository.RenameAsync(concept, Arguments.GetQuotation(Argument.DirectObject));

            writer.Write(_repository["Rename"]);

            concept.Write(writer);

            writer.Write('.');
        }

        public override Command CreateCommand(ArgumentSet arguments)
        {
            return new RenameCommand(arguments, _repository);
        }
    }
}
