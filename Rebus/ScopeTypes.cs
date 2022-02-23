// Ishan Pranav's REBUS: WriterScope.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;

namespace Rebus
{
    [Flags]
    public enum ScopeTypes
    {
        None = 0,
        DoubleQuotation = 1,
        Keyword = 2,
        Noun = 4,
        Parenthetical = 8,
        SingleQuotation = 16,
        VerbPhrase = 32
    }
}
