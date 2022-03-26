// Ishan Pranav's REBUS: RebusException.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Rebus.Exceptions
{
    [Serializable]
    public class RebusException : Exception
    {
        public RebusException(string? message) : base(message)
        {
        }

        protected RebusException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
