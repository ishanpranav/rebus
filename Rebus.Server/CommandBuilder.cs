// Ishan Pranav's REBUS: CommandBuilder.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    internal sealed class CommandBuilder : ICommandBuilder
    {
        private readonly Dictionary<Guid, Command> _prototypesByGuid;
        private readonly IDbContextFactory<ResourceContext> _contextFactory;
        private readonly DbRepository _repository;
        private readonly MessageBuilder _messageBuilder;
        private readonly IReadOnlyCollection<Argument> _arguments;
        private readonly List<Command> _commands = new List<Command>();
        private readonly Dictionary<Argument, object> _valuesByArgument = new Dictionary<Argument, object>();

        private IToken? _verb;
        private IToken? _adverb;
        private IPlayer? _player;
        private IConcept? _playerConcept;

        public CommandBuilder(IEnumerable<Command> prototypes, IDbContextFactory<ResourceContext> contextFactory, DbRepository repository, MessageBuilder messageBuilder, IReadOnlyCollection<Argument> arguments)
        {
            _prototypesByGuid = prototypes.ToDictionary(x => x.Guid);
            _contextFactory = contextFactory;
            _repository = repository;
            _messageBuilder = messageBuilder;
            _arguments = arguments;
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
            if (_playerConcept is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                _valuesByArgument[argument] = _playerConcept;
            }
        }

        public async Task SetConceptAsync(Argument argument, IEnumerable<IToken> adjectives, IToken substantive)
        {
            _valuesByArgument[argument] = await _repository.GetConceptAsync(article: null, adjectives, substantive);
        }

        public void SetQuotation(Argument argument, string quotation)
        {
            _valuesByArgument[argument] = quotation;
        }

        public void SetNumber(Argument argument, int number)
        {
            _valuesByArgument[argument] = number;
        }

        public async Task SaveChangesAsync()
        {
            if (_verb is null || _playerConcept is null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                await using (ResourceContext context = await _contextFactory.CreateDbContextAsync())
                {
                    string? adverb = _adverb?.Value;

                    try
                    {
                        CommandSignature? result = context.CommandSignatures
                            .Include(signature => signature.Command)
                            .Include(signature => signature.Arguments)
                            .Where(signature => signature.Verb == _verb.Value && signature.Adverb == adverb)
                            .AsEnumerable()
                            .SingleOrDefault(signature =>
                            {
                                Dictionary<Argument, ArgumentSignature> argumentSignaturesByArgument = signature.Arguments.ToDictionary(x => x.Argument);

                                foreach (Argument argument in _arguments)
                                {
                                    if (argumentSignaturesByArgument.TryGetValue(argument, out ArgumentSignature? argumentSignature))
                                    {
                                        if (_valuesByArgument.TryGetValue(argument, out object? value))
                                        {
                                            switch (argumentSignature.Type)
                                            {
                                                case ArgumentType.Concept:
                                                    if (value is not IConcept)
                                                    {
                                                        return false;
                                                    }
                                                    break;

                                                case ArgumentType.Number:
                                                    if (value is not int)
                                                    {
                                                        return false;
                                                    }
                                                    break;

                                                case ArgumentType.Quotation:
                                                    if (value is not string)
                                                    {
                                                        return false;
                                                    }
                                                    break;

                                                case ArgumentType.Reflexive:
                                                    if (value != _playerConcept)
                                                    {
                                                        return false;
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else if (_valuesByArgument.ContainsKey(argument))
                                    {
                                        return false;
                                    }
                                }

                                return true;
                            });

                        if (result is null)
                        {
                            throw new RebusException(await _messageBuilder.BuildAsync(ResourceIndex.CommandNoMatchesException));
                        }
                        else
                        {
                            Command clone = _prototypesByGuid[result.Command.Guid].Clone(_playerConcept);

                            foreach (KeyValuePair<Argument, object> argumentValuePair in _valuesByArgument)
                            {
                                clone.Set(argumentValuePair.Key, argumentValuePair.Value);
                            }

                            _commands.Add(clone);
                        }
                    }
                    catch
                    {
                        throw new RebusException(await _messageBuilder.BuildAsync(ResourceIndex.CommandMultipleMatchesException));
                    }
                }

                _verb = null;
                _adverb = null;
                _playerConcept = null;

                _valuesByArgument.Clear();
            }
        }

        public IEnumerable<Command> Build()
        {
            return _commands;
        }
    }
}
