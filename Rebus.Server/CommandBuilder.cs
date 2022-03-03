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

        private ArgumentSet _arguments = new ArgumentSet();

        public IArgumentSet Arguments
        {
            get
            {
                return _arguments;
            }
        }

        public IToken? Verb { get; set; }
        public IToken? Adverb { get; set; }

        private Player? _player;

        public CommandBuilder(RebusDbContext context, IEnumerable<Command> commands)
        {
            _context = context;
            _commandsByGuid = commands.ToDictionary(x => x.Guid);
        }

        public async Task SetPlayerAsync(int id)
        {
            _commands.Clear();
            _arguments = new ArgumentSet();

            _player = await _context.Players
                .IncludeSignatures()
                .SingleAsync(x => x.Id == id);

            if (_player.Signature is not null)
            {
                _arguments.Player = _player;
            }
        }

        public void Set(Argument argument, IReadOnlyCollection<IToken> adjectives, IToken substantive)
        {
            ConceptSignature? signature;

            try
            {
                signature = _context.ConceptSignatures
                    .Include(signature => signature.Substantive)
                    .Where(signature => signature.Substantive.Value == substantive.Value)
                    .Include(signature => signature.Article)
                    .Include(signature => signature.Adjectives)
                    .AsEnumerable()
                    .SingleOrDefault(signature => adjectives.All(adjective => signature.Adjectives.Any(x => adjective.Value == x.Value)));
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
                _arguments.SetConceptSignature(argument, signature);
            }
        }

        public void MoveNext()
        {
            if (Verb is null || _player is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                Command? prototype;

                try
                {
                    string? adverb = Adverb?.Value;

                    prototype = _context.CommandSignatures
                        .Where(signature => signature.VerbValue == Verb.Value && signature.AdverbValue == adverb)
                        .Select(x => x.Guid)
                        .AsEnumerable()
                        .Select(x => _commandsByGuid[x])
                        .SingleOrDefault(x => x.Matches(_arguments));
                }
                catch
                {
                    throw new RebusException(resource: 9);
                }

                _commands.Add((prototype ?? throw new RebusException(resource: 10)).CreateCommand(_arguments));
            }

            Verb = null;
            Adverb = null;
            _player = null;
        }

        public IEnumerable<Command> Build()
        {
            return _commands;
        }
    }
}
