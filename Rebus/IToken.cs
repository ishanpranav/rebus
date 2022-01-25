// Ishan Pranav's REBUS: IToken.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

namespace Rebus
{
    public interface IToken : IWritable
    {
        TokenTypes Type { get; }
        string Value { get; }
    }
}
