// Ishan Pranav's REBUS: TokenTypes.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Rebus
{
    /// <summary>
    /// Specifies the types of tokens.
    /// </summary>
    /// <remarks>The numeric values of the tokens are ordered by their precedence in the <see href="https://www.grammarly.com/blog/adjective-order/">order of cumulative adjectives in English</see>.</remarks>
    /// <seealso href="https://www.grammarly.com/blog/adjective-order/">Adjective Order in English</seealso>
    [Flags]
    public enum TokenTypes
    {
        /// <summary>
        /// An undefined token.
        /// </summary>
        None,

        /// <summary>
        /// An adjective.
        /// </summary>
        Adjective = 1024,

        /// <summary>
        /// An adverb.
        /// </summary>
        Adverb = 2048,

        /// <summary>
        /// An adjective that specifies the age of a noun.
        /// </summary>
        Age = 32,

        /// <summary>
        /// An article.
        /// </summary>
        Article = 1,

        /// <summary>
        /// An adjective that specifies the color of a noun.
        /// </summary>
        Color = 128,

        /// <summary>
        /// A conjunction.
        /// </summary>
        Conjunction = 4096,

        /// <summary>
        /// A determiner.
        /// </summary>
        Determiner = 2,

        /// <summary>
        /// A first person object pronoun.
        /// </summary>
        FirstPersonObject = 16384,

        /// <summary>
        /// A first person subject pronoun.
        /// </summary>
        FirstPersonSubject = 32768,

        /// <summary>
        /// An interjection.
        /// </summary>
        Interjection = 8192,

        /// <summary>
        /// An adjective that specifies the material of a noun. 
        /// </summary>
        Material = 512,

        /// <summary>
        /// A number.
        /// </summary>
        Number = 4,

        /// <summary>
        /// An object pronoun.
        /// </summary>
        Object = FirstPersonObject | SecondPersonObject,

        /// <summary>
        /// An adjective that specifies an opinion about a noun.
        /// </summary>
        Opinion = 8,

        /// <summary>
        /// An adjective that specifies the origin of a noun.
        /// </summary>
        Origin = 256,

        /// <summary>
        /// A string literal.
        /// </summary>
        Quotation = 65536,

        /// <summary>
        /// A second person object pronoun.
        /// </summary>
        SecondPersonObject = 131072,

        /// <summary>
        /// An adjective that specifies the shape of a noun.
        /// </summary>
        Shape = 64,

        /// <summary>
        /// An adjective that specifies the size of a noun.
        /// </summary>
        Size = 16,

        /// <summary>
        /// A noun substantive.
        /// </summary>
        Substantive = 262144,

        /// <summary>
        /// A symbol.
        /// </summary>
        Symbol = 524288,

        /// <summary>
        /// A verb.
        /// </summary>
        Verb = 1048576
    }
}
