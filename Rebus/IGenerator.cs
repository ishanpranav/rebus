// Ishan Pranav's REBUS: IGenerator.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

namespace Rebus
{
    public interface IGenerator
    {
        void Add(string value);
        string Generate();
    }
}
