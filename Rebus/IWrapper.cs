// Ishan Pranav's REBUS: IWrapper.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Text;

namespace Rebus
{
    public interface IWrapper
    {
        void Wrap(StringBuilder value);
    }
}
