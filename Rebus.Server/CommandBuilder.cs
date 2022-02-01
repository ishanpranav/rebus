// Ishan Pranav's REBUS: CommandBuilder.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rebus.Server
{
    internal sealed class CommandBuilder : ICommandBuilder
    {
        private readonly Dictionary<Guid, Command> _prototypesByGuid;
        private readonly Repository _repository;
        private readonly List<Command> _commands = new List<Command>();
        private readonly int _argumentCount = Enum.GetValues<Argument>().Length;

        private IToken? _verb;
        private IToken? _adverb;
        private IPlayer? _player;
        private IConcept? _playerConcept;
        private object?[] _arguments;

        public CommandBuilder(IEnumerable<Command> prototypes, Repository repository)
        {
            _prototypesByGuid = prototypes.ToDictionary(x => x.Guid);
            _repository = repository;
            _arguments = new object?[_argumentCount];
        }

        public void SetPlayer(IPlayer player)
        {
            _player = player;

            _commands.Clear();
        }

        public async Task SetVerbPhraseAsync(IToken verb, IToken? adverb)
        {
            _verb = verb;
            _adverb = adverb;

            if (_player is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                _playerConcept = await _repository.GetConceptAsync(_player.ConceptId);
            }
        }

        public void SetReflexive(Argument argument)
        {
            if (argument is Argument.Subject)
            {
                _arguments[(int)Argument.Subject] = _playerConcept;
            }
            else
            {
                _arguments[(int)argument] = _arguments[(int)Argument.Subject];
            }
        }

        public async Task SetConceptAsync(Argument argument, IEnumerable<IToken> adjectives, IToken substantive)
        {
            _arguments[(int)argument] = await _repository.GetConceptAsync(adjectives, substantive);
        }

        public void SetQuotation(Argument argument, string quotation)
        {
            _arguments[(int)argument] = quotation;
        }

        public void SetNumber(Argument argument, int number)
        {
            _arguments[(int)argument] = number;
        }

        public async Task SaveChangesAsync()
        {
            if (_verb is null || _playerConcept is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                _commands.Add(_prototypesByGuid[(await _repository.GetCommandAsync(_verb, _adverb, _arguments)).Command.Guid].CreateCommand(_playerConcept, _arguments));
            }

            _verb = null;
            _adverb = null;
            _playerConcept = null;
            _arguments = new object?[_argumentCount];
        }

        public IEnumerable<Command> Build()
        {
            return _commands;
        }
    }
}
