// Ishan Pranav's REBUS: 
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

namespace Rebus.Server
{
    internal interface IEntity : IWritable
    {
        public Concept Concept { get; }
    }
}
