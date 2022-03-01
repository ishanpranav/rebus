// Ishan Pranav's REBUS: RebusSpellingException.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Rebus.Exceptions
{
    [Serializable]
    public class RebusSpellingException : Exception
    {
        public TokenTypes ExpectedType
        {
            get
            {
                return (TokenTypes)(Data[nameof(ExpectedType)] ?? TokenTypes.None);
            }
        }

        public string? ActualValue
        {
            get
            {
                return Data[nameof(ActualValue)] as string;
            }
        }

        public RebusSpellingException(TokenTypes expectedType, string? actualValue)
        {
            Data[nameof(ExpectedType)] = expectedType;
            Data[nameof(ActualValue)] = actualValue;
        }

        protected RebusSpellingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
