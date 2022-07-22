// Ishan Pranav's REBUS: RpcService.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    public class RpcService : IGameService, ILoginService
    {
        private readonly IDbContextFactory<RebusDbContext> _contextFactory;
        private readonly Map _map;
        private readonly Namer _namer;

        public RpcService(IDbContextFactory<RebusDbContext> contextFactory, Map map, Namer namer)
        {
            _contextFactory = contextFactory;
            _map = map;
            _namer = namer;
        }

        public Task<int> GetRadiusAsync()
        {
            return Task.FromResult(_map.Radius);
        }

        public async Task<int> LoginAsync(string username, string password)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                await context.Database.EnsureCreatedAsync();

                return (await context.Players.SingleOrDefaultAsync(x => x.Username == username && x.Password == password))?.Id ?? default;
            }
        }

        public async Task<int> RegisterAsync(string username, string password)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                await context.Database.EnsureCreatedAsync();

                try
                {
                    Player player = new Player()
                    {
                        Username = username,
                        Password = password
                    };

                    await context.Players.AddAsync(player);

                    await context.Zones.AddAsync(new Zone()
                    {
                        Player = player,
                        Units = new Unit[]
                        {
                            new Unit()
                        }
                    });

                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    return default;
                }
            }

            return await LoginAsync(username, password);
        }

        public async IAsyncEnumerable<ZoneResult> GetZonesAsync(int playerId)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                //foreach (Zone zone in context.Zones
                //    .Where(x => x.PlayerId == playerId)
                //    .Include(x => x.Units))
                foreach (Zone zone in _map.Select(x => new Zone()
                {
                    Q = x.Q,
                    R = x.R,
                    PlayerId = 0,
                    Units = new Unit[]
                    {
                        new Unit()
                    }
                }))
                {
                    _map.Name(zone.Location, _namer, out string? name, out Depth depth);

                    yield return new ZoneResult(zone, depth, name);
                }
            }
        }

        public async Task<string> RenameAsync(int unitId, string name)
        {
            await using (RebusDbContext context = await _contextFactory.CreateDbContextAsync())
            {
                Unit unit = await context.Units.SingleAsync(x => x.Id == unitId);
                string previous = unit.Name;

                try
                {
                    unit.Name = name;

                    await context.SaveChangesAsync();

                    return name;
                }
                catch (DbUpdateException)
                {
                    return previous;
                }
            }
        }

        public Task MoveAsync(int unitId, HexPoint destination)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing) { }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
