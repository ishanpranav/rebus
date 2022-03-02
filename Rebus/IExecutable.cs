// Ishan Pranav's REBUS: IExecutable.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus
{
    public interface IExecutable
    {
        IAsyncEnumerable<IWritable> ExecuteAsync();
    }
}
