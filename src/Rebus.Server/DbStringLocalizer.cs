// Ishan Pranav's REBUS: DbStringLocalizer.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Rebus.Server
{
    internal sealed class DbStringLocalizer : IStringLocalizer
    {
        private readonly IFormatProvider _formatProvider;
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;

        public DbStringLocalizer(IFormatProvider formatProvider, IDbContextFactory<RebusDbContext> contextFactory)
        {
            _formatProvider = formatProvider;
            _contextFactory = contextFactory;
        }

        private string? GetResource(string name, int arguments)
        {
            using (RebusDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Resources
                    .Where(x => x.Key == name && x.Arguments == arguments)
                    .Select(x => x.Value)
                    .OrderBy(x => EF.Functions.Random())
                    .FirstOrDefault();
            }
        }

        public LocalizedString this[string name]
        {
            get
            {
                string? resource = GetResource(name, arguments: 0);

                if (resource is null)
                {
                    return new LocalizedString(name, name, resourceNotFound: true);
                }
                else
                {
                    return new LocalizedString(name, resource);
                }
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                string? resource = GetResource(name, arguments.Length);

                if (resource is null)
                {
                    return new LocalizedString(name, name, resourceNotFound: true);
                }
                else
                {
                    return new LocalizedString(name, string.Format(_formatProvider, resource, arguments));
                }
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            using (RebusDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Resources
                    .AsEnumerable()
                    .Select(x => new LocalizedString(x.Key, x.Value))
                    .ToArray();
            }
        }
    }
}
