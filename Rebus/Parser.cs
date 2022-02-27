// Ishan Pranav's REBUS: Parser.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;
using Rebus.Expressions;

namespace Rebus
{
    public class Parser
    {
        private readonly MessageFactory _messageBuilder;

        public Parser(MessageFactory messageBuilder)
        {
            _messageBuilder = messageBuilder;
        }

        public async Task<Expression> ParseAsync(IAsyncEnumerable<IToken> tokens)
        {
            await using (IAsyncEnumerator<IToken> enumerator = tokens.GetAsyncEnumerator())
            {
                await enumerator.MoveNextAsync();

                bool complete = false;
                Expression? result = await parseSentenceAsync();

                if (await acceptAsync(TokenTypes.Conjunction) is not null)
                {
                    List<Expression> sentences = new List<Expression>(2)
                {
                    result,
                    await parseSentenceAsync()
                };

                    while (await acceptAsync(TokenTypes.Conjunction) is not null)
                    {
                        sentences.Add(await parseSentenceAsync());
                    }

                    result = new ParagraphExpression(sentences);
                }

                if (complete)
                {
                    return result;
                }
                else
                {
                    throw await _messageBuilder.CreateExceptionAsync(resource: 1);
                }

                Task<RebusException> createSubstantiveExceptionAsync()
                {
                    return _messageBuilder.CreateExceptionAsync(resource: 4);
                }

                async Task<IToken?> acceptAsync(TokenTypes tokenType)
                {
                    if (!complete)
                    {
                        IToken result = enumerator.Current;

                        if (result is null)
                        {
                            throw await _messageBuilder.CreateExceptionAsync(resource: 2);
                        }
                        else if (result.Type.HasFlag(tokenType))
                        {
                            if (!await enumerator.MoveNextAsync())
                            {
                                complete = true;
                            }

                            return result;
                        }
                    }

                    return null;
                }

                async Task<Expression> parseSentenceAsync()
                {
                    Expression subject = await parseSubjectAsync();

                    while ((await acceptAsync(TokenTypes.Interjection)) is not null) { }

                    IToken verb = await acceptAsync(TokenTypes.Verb) ?? throw await _messageBuilder.CreateExceptionAsync(resource: 3);
                    IToken? adverb = await acceptAsync(TokenTypes.Adverb);

                    Expression? directObject = await parseDirectObjectAsync();

                    return new SentenceExpression(subject, new VerbPhraseExpression(verb, adverb ?? await acceptAsync(TokenTypes.Adverb)), directObject);
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
                                throw await createSubstantiveExceptionAsync();
                            }
                        }
                        else
                        {
                            return new NounExpression(Argument.Subject, article: null, adjectives, substantive);
                        }
                    }

                    return new ReflexiveExpression(Argument.Subject, firstPerson: true);
                }

                async Task<Expression?> parseDirectObjectAsync()
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
                                            throw await createSubstantiveExceptionAsync();
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
                            return new ReflexiveExpression(Argument.DirectObject, firstPerson: false);
                        }
                    }
                    else
                    {
                        return new ReflexiveExpression(Argument.DirectObject, firstPerson: true);
                    }
                }
            }
        }
    }
}
