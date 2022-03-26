// Ishan Pranav's REBUS: RebusSpellingException.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Rebus.Exceptions
{
    [Serializable]
    public class RebusSpellingException : Exception
    {
        public TokenTypes? ExpectedType
        {
            get
            {
                return (TokenTypes?)Data[nameof(ExpectedType)];
            }
        }

        public string? ActualValue
        {
            get
            {
                return Data[nameof(ActualValue)] as string;
            }
        }

        public RebusSpellingException(string? actualValue)
        {
            Data[nameof(ActualValue)] = actualValue;
        }

        public RebusSpellingException(TokenTypes expectedType, string? actualValue) : this(actualValue)
        {
            Data[nameof(ExpectedType)] = expectedType;
        }

        protected RebusSpellingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
