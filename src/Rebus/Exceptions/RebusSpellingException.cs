// Ishan Pranav's REBUS: RebusSpellingException.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Rebus.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the interpreter encounters a spelling error.
    /// </summary>
    [Serializable]
    public class RebusSpellingException : Exception
    {
        /// <summary>
        /// Gets the expected token type of the misspelled word.
        /// </summary>
        /// <value>The expected token type.</value>
        public TokenTypes? ExpectedType
        {
            get
            {
                return (TokenTypes?)Data[nameof(ExpectedType)];
            }
        }

        /// <summary>
        /// Gets the actual value of the misspelled word.
        /// </summary>
        /// <value>The actual value.</value>
        public string? ActualValue
        {
            get
            {
                return Data[nameof(ActualValue)] as string;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RebusSpellingException"/> class.
        /// </summary>
        /// <param name="actualValue">The actual value.</param>
        public RebusSpellingException(string? actualValue)
        {
            Data[nameof(ActualValue)] = actualValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RebusSpellingException"/> class.
        /// </summary>
        /// <param name="expectedType">The expected token type.</param>
        /// <param name="actualValue">The actual value.</param>
        public RebusSpellingException(TokenTypes expectedType, string? actualValue) : this(actualValue)
        {
            Data[nameof(ExpectedType)] = expectedType;
        }

        /// <inheritdoc/>
        protected RebusSpellingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
