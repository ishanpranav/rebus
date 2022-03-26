// Ishan Pranav's REBUS: IWritable.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    public interface IWritable
    {
        void Write(ExpressionWriter writer);
    }
}