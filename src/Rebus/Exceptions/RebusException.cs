// Ishan Pranav's REBUS: RebusException.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Rebus.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the engine encounters an error.
    /// </summary>
    [Serializable]
    public class RebusException : Exception
    {
        /// <inheritdoc/>
        public RebusException(string? message) : base(message) { }

        /// <inheritdoc/>
        protected RebusException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
