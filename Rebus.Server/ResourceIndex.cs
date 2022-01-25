// Ishan Pranav's REBUS: ResourceIndex.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

namespace Rebus.Server
{
    internal enum ResourceIndex
    {
        None,
        CommandMultipleMatchesException = 3,
        CommandNoMatchesException = 12,
        ConceptMultipleMatchesException = 1,
        ConceptNoMatchesException = 2,
        Failure = 11,
        ParserEmptyException = 5,
        ParserRemainderException = 6,
        ParserNounException = 7,
        ParserVerbException = 8,
        TransitiveVisionResponse = 9,
        VisionEmptyResponse = 10,
        VisionResponse = 4
    }
}
