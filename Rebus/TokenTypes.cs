// Ishan Pranav's REBUS: TokenTypes.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;

namespace Rebus
{
    [Flags]
    public enum TokenTypes
    {
        None = 0,
        Adjective = 1024,
        Adverb = 2048,
        Age = 32,
        Article = 1,
        Color = 128,
        Conjunction = 4096,
        Determiner = 2,
        Interjection = 8192,
        Material = 512,
        Number = 4,
        Object = 16384,
        Opinion = 8,
        Origin = 256,
        Quotation = 32768,
        Shape = 64,
        Size = 16,
        Subject = 65536,
        Substantive = 131072,
        Verb = 262144
    }
}