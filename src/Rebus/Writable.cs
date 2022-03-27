// Ishan Pranav's REBUS: Writable.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    /// <summary>
    /// Defines the core behavior of a writable object and provides a base for derived classes.	
    /// </summary>
    public abstract class Writable : IWritable
    {
        /// <summary>
        /// Called from constructors in derived classes to initialize the <see cref="Writable"/> class.
        /// </summary>
        protected Writable() { }

        /// <inheritdoc/>
        public abstract void Write(ExpressionWriter writer);

        /// <inheritdoc/>
        public override string ToString()
        {
            ExpressionWriter writer = new ExpressionWriter();

            Write(writer);

            return writer.ToString();
        }
    }
}
