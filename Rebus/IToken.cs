// Ishan Pranav's REBUS: IToken.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    public interface IToken : IWritable
    {
        TokenTypes Type { get; }
        string Value { get; }
    }
}
