// Ishan Pranav's REBUS: RebusException.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Rebus
{
    /// <summary>
    /// The exception that is thrown when the a user-provided game instruction contains a syntactical or semantic error.
    /// </summary>
    [Serializable]
    public class RebusException : Exception, IWritable
    {
        private readonly IWritable? _message;

        /// <summary>
        /// Initializes a new instance of the <see cref="RebusException"/> class.
        /// </summary>
        public RebusException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RebusException"/> class.
        /// </summary>
        /// <param name="message">The writable message that describes the error.</param>
        public RebusException(IWritable? message) : base(message?.ToString())
        {
            _message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RebusException"/> class.
        /// </summary>
        /// <inheritdoc cref="Exception(string)"/>
        public RebusException(string? message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RebusException"/> class.
        /// </summary>
        /// <inheritdoc cref="Exception(string, Exception)"/>
        public RebusException(string? message, Exception? innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RebusException"/> class.
        /// </summary>
        /// <inheritdoc cref="Exception(SerializationInfo, StreamingContext)"/>
        protected RebusException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <inheritdoc/>
        public void Write(ExpressionWriter writer)
        {
            if (_message is null)
            {
                writer.Write(Message);
            }
            else
            {
                _message.Write(writer);
            }
        }
    }
}
