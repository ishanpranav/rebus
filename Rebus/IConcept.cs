// Ishan Pranav's REBUS: IConcept.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

namespace Rebus
{
    public interface IConcept : IWritable
    {
        bool Is<T>() where T : class;
        bool Is<T>(out T? result) where T : class;
    }
}
