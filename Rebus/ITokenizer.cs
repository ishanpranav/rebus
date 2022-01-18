// Ishan Pranav's REBUS: ITokenizer.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus
{
    public interface ITokenizer
    {
        IAsyncEnumerable<IToken> TokenizeAsync(string value);
    }
}
