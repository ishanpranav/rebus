// Ishan Pranav's REBUS: CommandBuilder.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Exceptions;
using Rebus.Server.Concepts;

namespace Rebus.Server
{
    internal sealed class CommandBuilder : ICommandBuilder
    {
        private readonly RebusDbContext _context;
        private readonly List<Command> _commands = new List<Command>();
        private readonly Dictionary<Guid, Command> _commandsByGuid;
        private readonly Dictionary<Argument, object> _arguments = new Dictionary<Argument, object>();

        private int? _playerId;
        private Player? _player;

        public IToken? Verb { get; set; }
        public IToken? Adverb { get; set; }

        public CommandBuilder(RebusDbContext context, IEnumerable<Command> commands)
        {
            _context = context;
            _commandsByGuid = commands.ToDictionary(x => x.Guid);
        }

        public async Task SetPlayerAsync(int player)
        {
            _playerId = player;

            _commands.Clear();

            if (_playerId is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                _player = await _context.Players
                    .Include(x => x.Signatures)
                    .ThenInclude(x => x.Article)
                    .Include(x => x.Signatures)
                    .ThenInclude(x => x.Substantive)
                    .AsSplitQuery()
                    .Include(x => x.Signatures)
                    .ThenInclude(x => x.Adjectives)
                    .SingleAsync(x => x.Id == _playerId);
            }
        }

        public void SetReflexive(Argument argument)
        {
            if (argument is Argument.Subject)
            {
                _arguments[Argument.Subject] = true;
            }
            else
            {
                _arguments[argument] = _arguments[Argument.Subject];
            }
        }

        public void SetConceptSignature(Argument argument, IEnumerable<IToken> adjectives, IToken substantive)
        {
            ILookup<int, ConceptSignature> signaturesByCount = _context.ConceptSignatures
                .Include(signature => signature.Substantive)
                .Where(signature => signature.Substantive.Value == substantive.Value)
                .Include(signature => signature.Article)
                .Include(signature => signature.Spacecraft)
                .Include(signature => signature.Adjectives)
                .AsSplitQuery()
                .ToLookup(signature => adjectives.Count(adjective => signature.Adjectives.Any(x => adjective.Value == x.Value)));

            ConceptSignature? signature;

            try
            {
                signature = signaturesByCount[adjectives.Count()].SingleOrDefault();
            }
            catch
            {
                throw new RebusException(resource: 5);
            }

            if (signature is null)
            {
                throw new RebusException(resource: 8);
            }
            else
            {
                _arguments[argument] = signature;
            }
        }

        public void SetQuotation(Argument argument, string quotation)
        {
            _arguments[argument] = quotation;
        }

        public void SetNumber(Argument argument, int number)
        {
            _arguments[argument] = number;
        }

        public async Task SaveChangesAsync()
        {
            if (Verb is null || _player is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                CommandSignature? result;

                try
                {
                    result = await _context.CommandSignatures
                        .SingleOrDefaultAsync(signature => signature.Verb.Value == Verb.Value && signature.Adverb == Adverb);
                }
                catch
                {
                    throw new RebusException(resource: 9);
                }

                _commands.Add(_commandsByGuid[(result ?? throw new RebusException(resource: 10)).Guid].CreateCommand(_player, _arguments));
            }

            Verb = null;
            Adverb = null;
            _player = null;

            _arguments.Clear();
        }

        public IEnumerable<Command> Build()
        {
            return _commands;
        }
    }
}
