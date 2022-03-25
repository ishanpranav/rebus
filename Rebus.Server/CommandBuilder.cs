// Ishan Pranav's REBUS: CommandBuilder.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization;
using Rebus.Exceptions;

namespace Rebus.Server
{
    internal sealed class CommandBuilder : ICommandBuilder
    {
        private readonly Repository _repository;
        private readonly IStringLocalizer _localizer;
        private readonly List<Command> _commands = new List<Command>();
        private readonly Dictionary<Guid, Command> _commandsByGuid;

        private ArgumentSet _arguments;

        public IToken? Verb { get; set; }
        public IToken? Adverb { get; set; }
        public Argument Argument { get; set; }

        public CommandBuilder(int playerId, Repository repository, IEnumerable<Command> commands, IStringLocalizer localizer)
        {
            _repository = repository;
            _localizer = localizer;
            _commandsByGuid = commands.ToDictionary(x => x.GetType().GUID);

            _arguments = new ArgumentSet()
            {
                PlayerId = playerId
            };
        }

        public void Include(IReadOnlyCollection<IToken> adjectives, IToken substantive)
        {
            if (_arguments?.PlayerId is null or 0)
            {
                throw new InvalidOperationException();
            }
            else
            {
                foreach (Concept concept in _repository
             .GetConcepts(Argument, _arguments.PlayerId, adjectives, substantive))
                {
                    _arguments.AddConcept(Argument, concept);
                }

                if (_arguments.Empty())
                {
                    throw new RebusException(_localizer["UndefinedConcept"]);
                }
            }
        }

        public void Include(int number)
        {
            _arguments?.SetNumber(Argument, number);
        }

        public void Include(string quotation)
        {
            _arguments?.SetQuotation(Argument, quotation);
        }

        public void IncludeReflexive()
        {
            _arguments?.SetReflexive(Argument);
        }

        public void MoveNext()
        {
            if (Verb is null || _arguments?.PlayerId is null or 0)
            {
                throw new InvalidOperationException();
            }
            else
            {
                Command? prototype;

                try
                {
                    prototype = _repository.GetCommand(Verb.Value, Adverb?.Value, _commandsByGuid, _arguments);
                }
                catch
                {
                    throw new RebusException(_localizer["AmbiguousCommand"]);
                }

                _commands.Add((prototype ?? throw new RebusException(_localizer["UndefinedCommand"])).CreateCommand(_arguments));
            }

            Verb = null;
            Adverb = null;
            _arguments = new ArgumentSet()
            {
                PlayerId = _arguments.PlayerId
            };
        }

        public IEnumerable<Command> Build()
        {
            return _commands;
        }
    }
}
