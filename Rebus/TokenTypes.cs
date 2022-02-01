// Ishan Pranav's REBUS: TokenTypes.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;

namespace Rebus
{
    [Flags]
    public enum TokenTypes
    {
        None,
        Adjective = 1024,
        Adverb = 2048,
        Age = 32,
        Article = 1,
        Color = 128,
        Conjunction = 4096,
        Determiner = 2,
        FirstPersonObject = 16384,
        FirstPersonSubject = 32768,
        Interjection = 8192,
        Material = 512,
        Number = 4,
        Object = FirstPersonObject | SecondPersonObject,
        Opinion = 8,
        Origin = 256,
        Quotation = 65536,
        SecondPersonObject = 131072,
        Shape = 64,
        Size = 16,
        Substantive = 262144,
        Symbol = 524288,
        Verb = 1048576
    }
}
