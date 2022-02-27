// Ishan Pranav's REBUS: VisionCommand.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server.Commands
{
    [Guid("6C4E330D-29AD-498B-B6A9-CB45724B0A32")]
    internal sealed class VisionCommand : Command
    {
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;

        public VisionCommand(IDbContextFactory<RebusDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        protected override async IAsyncEnumerable<IWritable> ExecuteAsync()
        {
            IConcept subject = GetConcept(Argument.Subject);

            if (subject is IViewer viewer)
            {
                await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
                {
                    await foreach (IWritable writable in viewer.ViewAsync(context))
                    {
                        yield return writable;
                    }
                }
            }
        }
    }
}
