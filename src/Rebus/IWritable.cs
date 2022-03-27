// Ishan Pranav's REBUS: IWritable.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    /// <summary>
    /// Defines a method for writing an object to an expression writer.
    /// </summary>
    public interface IWritable
    {
        /// <summary>
        /// Writes the string representation of the current object too an expression writer.
        /// </summary>
        /// <param name="writer">The expression writer.</param>
        void Write(ExpressionWriter writer);
    }
}
