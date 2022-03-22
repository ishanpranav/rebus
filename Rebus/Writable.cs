// Ishan Pranav's REBUS: Writable.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    public abstract class Writable : IWritable
    {
        public abstract void Write(ExpressionWriter writer);

        public override string ToString()
        {
            ExpressionWriter writer = new ExpressionWriter();

            Write(writer);

            return writer.ToString();
        }
    }
}
