// Ishan Pranav's REBUS: Writable.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

namespace Rebus
{
    public abstract class Writable : IWritable
    {
        public abstract void Write(ExpressionWriter writer);

        public override string ToString()
        {
            StringExpressionWriter writer = new StringExpressionWriter();

            this.Write(writer);

            return writer.ToString();
        }
    }
}
