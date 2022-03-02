// Ishan Pranav's REBUS: IUnexecutable.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus
{
    public interface IUnexecutable
    {
        IAsyncEnumerable<IWritable> UnexecuteAsync();
    }
}
