// Ishan Pranav's REBUS: Parser.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Localization;
using Rebus.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebus
{
    public class Parser
    {
        private readonly IStringLocalizer<Parser> _stringLocalizer;
        private readonly ITokenizer _tokenizer;

        private IAsyncEnumerator<IToken>? _enumerator;
        //private CollectionExpression? _directObject;

        public Parser(IStringLocalizer<Parser> stringLocalizer, ITokenizer tokenizer)
        {
            this._stringLocalizer = stringLocalizer;
            this._tokenizer = tokenizer;
        }

        private async Task<IToken?> AcceptAsync(TokenTypes tokenType)
        {
            if (this._enumerator is not null)
            {
                IToken result = this._enumerator.Current;

                if (result is null)
                {
                    throw new RebusException(this._stringLocalizer["0"]);
                }
                else if (result.Type.HasFlag(tokenType))
                {
                    if (!await this._enumerator.MoveNextAsync())
                    {
                        await this._enumerator.DisposeAsync();

                        this._enumerator = null;
                    }

                    return result;
                }
            }

            return null;
        }

        public async Task<Expression> ParseAsync(string value)
        {
            await using (this._enumerator = this._tokenizer.TokenizeAsync(value).GetAsyncEnumerator())
            {
                await this._enumerator.MoveNextAsync();

                Expression result = await this.ParseSentenceAsync();

                if (await this.AcceptAsync(TokenTypes.Conjunction) is not null)
                {
                    List<Expression> sentences = new List<Expression>(2)
                    {
                        result,
                        await this.ParseSentenceAsync()
                    };

                    while (await this.AcceptAsync(TokenTypes.Conjunction) is not null)
                    {
                        sentences.Add(await this.ParseSentenceAsync());
                    }

                    result = new ParagraphExpression(sentences);
                }

                if (this._enumerator is null)
                {
                    return result;
                }
                else
                {
                    throw new RebusException(this._stringLocalizer["1f1", result]);
                }
            }
        }

        //private async Task<bool> CollectNounAsync()
        //{
        //    IExpression? noun = await this.ParseObjectAsync();

        //    if (noun is not null)
        //    {
        //        if (this._directObject is null)
        //        {
        //            throw new RebusException($"I expected a verb but you wrote the noun {noun}.");
        //        }
        //        else
        //        {
        //            this._directObject.Append(noun);

        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        private async Task<Expression> ParseSentenceAsync()
        {
            Expression? subject = await this.ParseSubjectAsync();

            IToken verb = await this.AcceptAsync(TokenTypes.Verb) ?? throw new RebusException(this._stringLocalizer["2"]);
            IToken? adverb = await this.AcceptAsync(TokenTypes.Adverb);

            Expression? directObject = await this.ParseDirectObjectAsync();

            //this._directObject = new CollectionExpression();

            //Expression? directObject = await this.ParseDirectObjectAsync();

            //if (item is null)
            //{
            //    IExpression? quotation = await this.ParseQuotationAsync();

            //    if (quotation is null)
            //    {
            //        return new ClauseExpression(new VerbPhraseExpression(verb, adverb ?? await this.ParseLiteralAsync(TokenTypes.Adverb)), directObject: null);
            //    }
            //    else
            //    {
            //        this._directObject.Append(quotation);
            //    }
            //}
            //else if (item != this._directObject)
            //{
            //    this._directObject.Append(item);
            //}

            return new SentenceExpression(subject, new VerbPhraseExpression(verb, adverb ?? await this.AcceptAsync(TokenTypes.Adverb)), directObject);
        }

        private async Task<Expression?> ParseSubjectAsync()
        {
            IToken? subject = await this.AcceptAsync(TokenTypes.Subject);

            if (subject is null)
            {
                List<IToken> adjectives = new List<IToken>();
                IToken? adjective;

                while ((adjective = await this.AcceptAsync(TokenTypes.Adjective)) is not null)
                {
                    adjectives.Add(adjective);
                }

                IToken? substantive = await this.AcceptAsync(TokenTypes.Substantive);

                if (substantive is null)
                {
                    if (adjectives.Count > 0)
                    {
                        throw new RebusException(this._stringLocalizer["3"]);
                    }
                }
                else
                {
                    return new SubjectExpression(adjectives, substantive);
                }
            }

            return null;
        }

        private async Task<Expression?> ParseDirectObjectAsync()
        {
            IToken? directObject = await this.AcceptAsync(TokenTypes.Object);

            if (directObject is null)
            {
                List<IToken> adjectives = new List<IToken>();
                IToken? adjective;

                while ((adjective = await this.AcceptAsync(TokenTypes.Adjective)) is not null)
                {
                    adjectives.Add(adjective);
                }

                IToken? substantive = await this.AcceptAsync(TokenTypes.Substantive);

                if (substantive is null)
                {
                    if (adjectives.Count > 0)
                    {
                        throw new RebusException(this._stringLocalizer["3"]);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return new DirectObjectExpression(adjectives, substantive);
                }
            }
            else
            {
                return new ReflexiveDirectObjectExpression();
            }
        }

        //private async Task<IExpression?> ParseQuotationAsync()
        //{
        //    string? none = await this.ParseLiteralAsync(TokenTypes.None);

        //    if (String.IsNullOrEmpty(none))
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return new QuotationExpression(none);
        //    }
        //}

        //private async Task<Expression?> ParseDirectObjectAsync()
        //{
        //    if (await this.AcceptAsync(TokenTypes.Object) is null)
        //    {
        //        string? quotation = await this.ParseLiteralAsync(TokenTypes.Quotation);

        //        if (quotation is null)
        //        {
        //            Expression? noun = await this.ParseNounAsync();

        //            if (noun is null)
        //            {
        //                return null;
        //            }
        //            else
        //            {
        //                return new DirectObjectExpression(noun);
        //            }
        //        }
        //        else
        //        {
        //            return new QuotationExpression(quotation);
        //        }
        //    }
        //    else
        //    {
        //        return new DirectObjectExpression(new PersonExpression());
        //    }
        //}

        //private async Task<int?> ParseNumberAsync()
        //{
        //    Token? result = await this.AcceptAsync(TokenTypes.Number);

        //    if (result is null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return Int32.Parse(result.Value);
        //    }
        //}

        //private async Task<Expression?> ParseNounAsync()
        //{
        //    int? number = null;

        //    if (await this.AcceptAsync(TokenTypes.Article) is null)
        //    {
        //        await this.AcceptAsync(TokenTypes.Demonstrative);

        //        number = await this.ParseNumberAsync();
        //    }

        //    List<string> adjectives = new List<string>();
        //    string? adjective;

        //    while ((adjective = await this.ParseLiteralAsync(TokenTypes.Adjective)) is not null)
        //    {
        //        adjectives.Add(adjective);
        //    }

        //    string? substantive = await this.ParseLiteralAsync(TokenTypes.Substantive);

        //    if (substantive is null)
        //    {
        //        if (adjectives.Count > 0 || number is not null)
        //        {
        //            throw new RebusException("I expected a noun.");
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        return new NounExpression(number ?? 1, adjectives, substantive);
        //    }
        //}
    }
}
