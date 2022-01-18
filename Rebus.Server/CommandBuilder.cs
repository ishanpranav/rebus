// Ishan Pranav's REBUS: CommandBuilder.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebus.Server
{
    internal class CommandBuilder : ICommandBuilder
    {
        private readonly string _playerTag;
        private readonly DbRepository _repository;
        private readonly CommandRepository _commandRepository;
        private readonly List<Command> _commands = new List<Command>();

        private IToken? _verb;
        private IToken? _adverb;
        private IConcept? _player;
        private IConcept? _subject;
        private IConcept? _directObject;
        private bool _reflexive;

        public CommandBuilder(string playerTag, DbRepository repository, CommandRepository commandRepository)
        {
            this._playerTag = playerTag;
            this._repository = repository;
            this._commandRepository = commandRepository;
        }

        public async Task SetVerbPhraseAsync(IToken verb, IToken? adverb)
        {
            this._verb = verb;
            this._adverb = adverb;
            this._player = await this._repository.GetPlayerAsync(this._playerTag);
        }

        public async Task SetSubjectAsync(IEnumerable<IToken> adjectives, IToken substantive)
        {
            this._subject = await this._repository.GetConceptAsync(adjectives, substantive, Characteristics.Agent);
        }

        public async Task SetDirectObjectAsync(IEnumerable<IToken> adjectives, IToken substantive)
        {
            this._directObject = await this._repository.GetConceptAsync(adjectives, substantive, Characteristics.None);
        }

        public void SetReflexive()
        {
            this._reflexive = true;
        }

        public void MoveNext()
        {
            if (this._verb is null || this._player is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                if (this._reflexive)
                {
                    this._directObject = this._player;
                }

                Command command = this._commandRepository
                    .GetCommand(this._verb, this._adverb, this._reflexive, this._directObject?.Characteristics ?? Characteristics.None)
                    .Clone(this._player, this._subject ?? this._player, this._directObject);

                this._commands.Add(command);

                this._verb = null;
                this._adverb = null;
                this._player = null;
                this._subject = null;
                this._directObject = null;
                this._reflexive = false;
            }
        }

        public IEnumerable<Command> Build()
        {
            return this._commands;
        }
    }
}
