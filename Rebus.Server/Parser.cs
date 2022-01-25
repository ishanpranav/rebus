// Ishan Pranav's REBUS: Parser.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;
using Rebus.Expressions;

namespace Rebus.Server
{
    internal sealed class Parser
    {
        private readonly Tokenizer _tokenizer;
        private readonly MessageBuilder _messageBuilder;

        private IAsyncEnumerator<Token>? _enumerator;

        public Parser(Tokenizer tokenizer, MessageBuilder messageBuilder)
        {
            _tokenizer = tokenizer;
            _messageBuilder = messageBuilder;
        }

        private async Task<Token?> AcceptAsync(TokenTypes tokenType)
        {
            if (_enumerator is not null)
            {
                Token result = _enumerator.Current;

                if (result is null)
                {
                    throw new RebusException(await _messageBuilder.BuildAsync(ResourceIndex.ParserEmptyException));
                }
                else if (result.Type.HasFlag(tokenType))
                {
                    if (!await _enumerator.MoveNextAsync())
                    {
                        await _enumerator.DisposeAsync();

                        _enumerator = null;
                    }

                    return result;
                }
            }

            return null;
        }

        public async Task<Expression> ParseAsync(string value)
        {
            await using (_enumerator = _tokenizer
                .TokenizeAsync(value)
                .GetAsyncEnumerator())
            {
                await _enumerator.MoveNextAsync();

                Expression result = await ParseSentenceAsync();

                if (await AcceptAsync(TokenTypes.Conjunction) is not null)
                {
                    List<Expression> sentences = new List<Expression>(2)
                    {
                        result,
                        await ParseSentenceAsync()
                    };

                    while (await AcceptAsync(TokenTypes.Conjunction) is not null)
                    {
                        sentences.Add(await ParseSentenceAsync());
                    }

                    result = new ParagraphExpression(sentences);
                }

                if (_enumerator is null)
                {
                    return result;
                }
                else
                {
                    _messageBuilder.Append(result);

                    throw new RebusException(await _messageBuilder.BuildAsync(ResourceIndex.ParserRemainderException));
                }
            }
        }

        private async Task<Expression> ParseSentenceAsync()
        {
            Expression? subject = await ParseSubjectAsync();

            Token verb = await AcceptAsync(TokenTypes.Verb) ?? throw new RebusException(await _messageBuilder.BuildAsync(ResourceIndex.ParserVerbException));
            Token? adverb = await AcceptAsync(TokenTypes.Adverb);

            Expression? directObject = await ParseDirectObjectAsync();

            return new SentenceExpression(subject, new VerbPhraseExpression(verb, adverb ?? await AcceptAsync(TokenTypes.Adverb)), directObject);
        }

        private async Task<Expression?> ParseSubjectAsync()
        {
            Token? subject = await AcceptAsync(TokenTypes.Subject);

            if (subject is null)
            {
                List<Token> adjectives = new List<Token>();
                Token? adjective;

                while ((adjective = await AcceptAsync(TokenTypes.Adjective)) is not null)
                {
                    adjectives.Add(adjective);
                }

                Token? substantive = await AcceptAsync(TokenTypes.Substantive);

                if (substantive is null)
                {
                    if (adjectives.Count > 0)
                    {
                        throw new RebusException(await _messageBuilder.BuildAsync(ResourceIndex.ParserNounException));
                    }
                }
                else
                {
                    return new NounExpression(Argument.Subject, article: null, adjectives, substantive);
                }
            }

            return null;
        }

        private async Task<Expression?> ParseDirectObjectAsync()
        {
            Token? @object = await AcceptAsync(TokenTypes.Object);

            if (@object is null)
            {
                Token? number = await AcceptAsync(TokenTypes.Number);

                if (number is null)
                {
                    Token? quotation = await AcceptAsync(TokenTypes.Quotation);

                    if (quotation is null)
                    {
                        Token? article = await AcceptAsync(TokenTypes.Article);
                        List<Token> adjectives = new List<Token>();
                        Token? adjective;

                        while ((adjective = await AcceptAsync(TokenTypes.Adjective)) is not null)
                        {
                            adjectives.Add(adjective);
                        }

                        Token? substantive = await AcceptAsync(TokenTypes.Substantive);

                        if (substantive is null)
                        {
                            if (adjectives.Count > 0)
                            {
                                throw new RebusException(await _messageBuilder.BuildAsync(ResourceIndex.ParserNounException));
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return new NounExpression(Argument.DirectObject, article, adjectives, substantive);
                        }
                    }
                    else
                    {
                        return new QuotationExpression(Argument.DirectObject, quotation.Value);
                    }
                }
                else
                {
                    return new NumberExpression(Argument.DirectObject, int.Parse(number.Value));
                }
            }
            else
            {
                return new ReflexiveExpression(Argument.DirectObject);
            }
        }
    }
}
