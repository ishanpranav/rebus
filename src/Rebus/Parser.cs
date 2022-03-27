// Ishan Pranav's REBUS: Parser.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Localization;
using Rebus.Exceptions;
using Rebus.Expressions;

namespace Rebus
{
    /// <summary>
    /// Represents a recursive descent parser that constructs an abstract syntax tree of <see cref="Expression"/> instances from a set of <see cref="IToken"/> instances.
    /// </summary>
    /// <remarks>
    /// The implementation of this class was inspired by and based on <see href="https://en.wikipedia.org/wiki/Recursive_descent_parser">this</see> Wikipedia article.
    /// </remarks>
    /// <seealso href="https://en.wikipedia.org/wiki/Recursive_descent_parser">Recursive descent parser - Wikipedia</seealso>
    public class Parser
    {
        private readonly IEnumerable<IToken> _tokens;
        private readonly IStringLocalizer _localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Parser"/> class.
        /// </summary>
        /// <param name="tokens">The input tokens.</param>
        /// <param name="localizer">The string localizer.</param>
        public Parser(IEnumerable<IToken> tokens, IStringLocalizer localizer)
        {
            _tokens = tokens;
            _localizer = localizer;
        }

        /// <summary>
        /// Constructs an abstract syntax tree.
        /// </summary>
        /// <returns>The abstract syntax tree as an <see cref="Expression"/> instance.</returns>
        /// <exception cref="RebusSpellingException">A spelling error was encountered.</exception>
        /// <exception cref="RebusException">An error occured while parsing.</exception>
        public Expression Parse()
        {
            using (IEnumerator<IToken> enumerator = _tokens.GetEnumerator())
            {
                enumerator.MoveNext();

                bool complete = false;
                string? actualValue = null;
                Expression? result = parseSentence();

                if (accept(TokenTypes.Conjunction) is not null)
                {
                    result = parseRequiredCollection(parseSentence);
                }

                if (complete)
                {
                    return result;
                }
                else
                {
                    throw new RebusSpellingException(actualValue);
                }

                IToken? accept(TokenTypes tokenType)
                {
                    if (!complete)
                    {
                        IToken? result = enumerator.Current;

                        if (result is null)
                        {
                            throw new RebusException(_localizer["EmptyParsingError"]);
                        }
                        else
                        {
                            actualValue = result.Value;

                            if (result.Type.HasFlag(tokenType))
                            {
                                if (!enumerator.MoveNext())
                                {
                                    complete = true;
                                }

                                return result;
                            }
                        }
                    }

                    return null;
                }

                Expression parseSentence()
                {
                    Expression subject = parseRequiredCollection(parseSubject);
                    Dictionary<Argument, Expression> nouns = new Dictionary<Argument, Expression>()
                    {
                        { Argument.Subject, subject }
                    };

                    while ((accept(TokenTypes.Interjection)) is not null) { }

                    IToken verb = accept(TokenTypes.Verb) ?? throw new RebusSpellingException(TokenTypes.Verb, actualValue);
                    IToken? adverb = accept(TokenTypes.Adverb);

                    Expression? firstObject = parseCollection(parseObject);
                    Expression? secondObject = parseCollection(parseObject);

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

                    return new SentenceExpression(nouns, new VerbPhraseExpression(verb, adverb ?? accept(TokenTypes.Adverb)));
                }

                Expression parseSubject()
                {
                    IToken? subject = accept(TokenTypes.FirstPersonSubject);

                    if (subject is null)
                    {
                        List<IToken> adjectives = new List<IToken>();
                        IToken? adjective;

                        while ((adjective = accept(TokenTypes.Adjective)) is not null)
                        {
                            adjectives.Add(adjective);
                        }

                        IToken? substantive = accept(TokenTypes.Substantive);

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

                Expression parseRequiredCollection(Func<Expression> itemParser)
                {
                    List<Expression> sentences = new List<Expression>();

                    do
                    {
                        sentences.Add(itemParser());
                    }
                    while (accept(TokenTypes.Conjunction) is not null);

                    return new CollectionExpression(sentences);
                }

                Expression? parseCollection(Func<Expression?> itemParser)
                {
                    List<Expression> sentences = new List<Expression>();

                    do
                    {
                        Expression? item = itemParser();

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
                    while (accept(TokenTypes.Conjunction) is not null);

                    return new CollectionExpression(sentences);
                }

                Expression? parseObject()
                {
                    if (accept(TokenTypes.FirstPersonObject) is null)
                    {
                        if (accept(TokenTypes.SecondPersonObject) is null)
                        {
                            IToken? number = accept(TokenTypes.Number);

                            if (number is null)
                            {
                                IToken? quotation = accept(TokenTypes.Quotation);

                                if (quotation is null)
                                {
                                    IToken? article = accept(TokenTypes.Article);
                                    List<IToken> adjectives = new List<IToken>();
                                    IToken? adjective;

                                    while ((adjective = accept(TokenTypes.Adjective)) is not null)
                                    {
                                        adjectives.Add(adjective);
                                    }

                                    IToken? substantive = accept(TokenTypes.Substantive);

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
