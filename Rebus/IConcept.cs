// Ishan Pranav's REBUS: IConcept.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

namespace Rebus
{
    public interface IConcept : IWritable
    {
        int Id { get; }
        int? ContainerId { get; }
        Characteristics Characteristics { get; }
        string VisualDescription { get; }
    }
}
