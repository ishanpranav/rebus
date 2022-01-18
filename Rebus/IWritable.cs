// Ishan Pranav's REBUS: IWritable.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

namespace Rebus
{
    public interface IWritable
    {
        void Write(ExpressionWriter writer);
    }
}