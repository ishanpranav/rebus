// Ishan Pranav's REBUS: Parser.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rebus.Exceptions;
using Rebus.Expressions;

namespace Rebus
{
    /// <summary>
    /// Provides a recursive descent parser that constructs an abstract syntax tree of <see cref="Expression"/> instances from a set of <see cref="IToken"/> instances.
    /// </summary>
    /// <remarks>
    /// The implementation of this class was inspired by and based on <see href="https://en.wikipedia.org/wiki/Recursive_descent_parser">this</see> Wikipedia article.
    /// </remarks>
    /// <seealso href="https://en.wikipedia.org/wiki/Recursive_descent_parser">Recursive descent parser - Wikipedia</seealso>
    public class Parser
    {
        private readonly IAsyncEnumerable<IToken> _tokens;

        public Parser(IAsyncEnumerable<IToken> tokens)
        {
            _tokens = tokens;
        }

        public async Task<Expression> ParseAsync()
        {
            await using (IAsyncEnumerator<IToken> enumerator = _tokens.GetAsyncEnumerator())
            {
                await enumerator.MoveNextAsync();

                bool complete = false;
                string? actualValue = null;
                Expression? result = await parseSentenceAsync();

                if (await acceptAsync(TokenTypes.Conjunction) is not null)
                {
                    result = await parseRequiredCollectionAsync(parseSentenceAsync);
                }

                if (complete)
                {
                    return result;
                }
                else
                {
                    throw new RebusSpellingException(actualValue);
                }

                async Task<IToken?> acceptAsync(TokenTypes tokenType)
                {
                    if (!complete)
                    {
                        IToken? result = enumerator.Current;

                        if (result is null)
                        {
                            throw new RebusException(resource: 2);
                        }
                        else
                        {
                            actualValue = result.Value;

                            if (result.Type.HasFlag(tokenType))
                            {
                                if (!await enumerator.MoveNextAsync())
                                {
                                    complete = true;
                                }

                                return result;
                            }
                        }
                    }

                    return null;
                }

                async Task<Expression> parseSentenceAsync()
                {
                    Expression subject = await parseRequiredCollectionAsync(parseSubjectAsync);
                    Dictionary<Argument, Expression> nouns = new Dictionary<Argument, Expression>()
                    {
                        { Argument.Subject, subject }
                    };

                    while ((await acceptAsync(TokenTypes.Interjection)) is not null) { }

                    IToken verb = await acceptAsync(TokenTypes.Verb) ?? throw new RebusSpellingException(TokenTypes.Verb, actualValue);
                    IToken? adverb = await acceptAsync(TokenTypes.Adverb);

                    Expression? firstObject = await parseCollectionAsync(parseObjectAsync);
                    Expression? secondObject = await parseCollectionAsync(parseObjectAsync);

                    if (firstObject is not null)
                    {
                        if (secondObject is null)
                        {
                            nouns.Add(Argument.DirectObject, firstObject);
                        }
                        else
                        {
                            nouns.Add(Argument.IndirectObject, firstObject);
                            nouns.Add(Argument.DirectObject, secondObject);
                        }
                    }

                    return new SentenceExpression(nouns, new VerbPhraseExpression(verb, adverb ?? await acceptAsync(TokenTypes.Adverb)));
                }

                async Task<Expression> parseSubjectAsync()
                {
                    IToken? subject = await acceptAsync(TokenTypes.FirstPersonSubject);

                    if (subject is null)
                    {
                        List<IToken> adjectives = new List<IToken>();
                        IToken? adjective;

                        while ((adjective = await acceptAsync(TokenTypes.Adjective)) is not null)
                        {
                            adjectives.Add(adjective);
                        }

                        IToken? substantive = await acceptAsync(TokenTypes.Substantive);

                        if (substantive is null)
                        {
                            if (adjectives.Count > 0)
                            {
                                throw new RebusSpellingException(TokenTypes.Substantive, actualValue);
                            }
                        }
                        else
                        {
                            return new NounExpression(article: null, adjectives, substantive);
                        }
                    }

                    return new ReflexiveExpression(firstPerson: true);
                }

                async Task<Expression> parseRequiredCollectionAsync(Func<Task<Expression>> parseItemAsyncFunction)
                {
                    List<Expression> sentences = new List<Expression>();

                    do
                    {
                        sentences.Add(await parseItemAsyncFunction());
                    }
                    while (await acceptAsync(TokenTypes.Conjunction) is not null);

                    return new CollectionExpression(sentences);
                }

                async Task<Expression?> parseCollectionAsync(Func<Task<Expression?>> parseItemAsyncFunction)
                {
                    List<Expression> sentences = new List<Expression>();

                    do
                    {
                        Expression? item = await parseItemAsyncFunction();

                        if (item is null)
                        {
                            if (sentences.Count == 0)
                            {
                                return null;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            sentences.Add(item);
                        }
                    }
                    while (await acceptAsync(TokenTypes.Conjunction) is not null);

                    return new CollectionExpression(sentences);
                }

                async Task<Expression?> parseObjectAsync()
                {
                    if (await acceptAsync(TokenTypes.FirstPersonObject) is null)
                    {
                        if (await acceptAsync(TokenTypes.SecondPersonObject) is null)
                        {
                            IToken? number = await acceptAsync(TokenTypes.Number);

                            if (number is null)
                            {
                                IToken? quotation = await acceptAsync(TokenTypes.Quotation);

                                if (quotation is null)
                                {
                                    IToken? article = await acceptAsync(TokenTypes.Article);
                                    List<IToken> adjectives = new List<IToken>();
                                    IToken? adjective;

                                    while ((adjective = await acceptAsync(TokenTypes.Adjective)) is not null)
                                    {
                                        adjectives.Add(adjective);
                                    }

                                    IToken? substantive = await acceptAsync(TokenTypes.Substantive);

                                    if (substantive is null)
                                    {
                                        if (adjectives.Count > 0)
                                        {
                                            throw new RebusSpellingException(TokenTypes.Substantive, actualValue);
                                        }
                                        else
                                        {
                                            return null;
                                        }
                                    }
                                    else
                                    {
                                        return new NounExpression(article, adjectives, substantive);
                                    }
                                }
                                else
                                {
                                    return new QuotationExpression(quotation.Value);
                                }
                            }
                            else
                            {
                                return new NumberExpression(int.Parse(number.Value));
                            }
                        }
                        else
                        {
                            return new ReflexiveExpression(firstPerson: false);
                        }
                    }
                    else
                    {
                        return new ReflexiveExpression(firstPerson: true);
                    }
                }
            }
        }
    }
}
