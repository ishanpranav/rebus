// Ishan Pranav's REBUS: IEngine.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Rebus.Server
{
    internal interface IEngine
    {
        ILogger Logger { get; }
        IDbContextFactory<RebusDbContext> DbContextFactory { get; }
        IEnumerable<Command> Commands { get; }
        IEditDistance EditDistance { get; }

        void LogExpression(Expression value);
    }
}
