// Ishan Pranav's REBUS: 
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Rebus.Server
{
    internal interface IViewer
    {
        IAsyncEnumerable<IWritable> ViewAsync(RebusDbContext context);
    }
}
