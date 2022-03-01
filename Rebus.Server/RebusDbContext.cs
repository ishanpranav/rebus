// Ishan Pranav's REBUS: RebusDbContext.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Rebus.Server
{
    internal sealed class RebusDbContext : DbContext, ITokenFactory
    {
#nullable disable
        public DbSet<CommandSignature> CommandSignatures { get; set; }
        public DbSet<Concept> Concepts { get; set; }
        public DbSet<ConceptSignature> ConceptSignatures { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Spacecraft> Spacecraft { get; set; }
        public DbSet<Token> Tokens { get; set; }
#nullable enable

        public RebusDbContext(DbContextOptions<RebusDbContext> options) : base(options) { }

        public IQueryable<Concept> IncludeConcepts()
        {
            return Concepts
                .Include(x => x.Signatures)
                .ThenInclude(x => x.Article)
                .Include(x => x.Signatures)
                .ThenInclude(x => x.Substantive)
                .AsSplitQuery()
                .Include(x => x.Signatures)
                .ThenInclude(x => x.Adjectives);
        }

        public Task<IWritable> CreateMessageAsync(int resource, params object?[] arguments)
        {
            return CreateMessageAsync(player: null, subject: null, resource, arguments);
        }

        public async Task<IWritable> CreateMessageAsync(IConcept? player, IConcept? subject, int resource, params object?[] arguments)
        {
            object[] filteredArguments = arguments
                .OfType<object>()
                .Where(x => x is not IEnumerable enumerable || enumerable
                    .Cast<object>()
                    .Any())
                .ToArray();
            int count = filteredArguments.Length;

            return new Message(player, subject, await Resources
                .Where(x => x.Key == resource && x.Arguments == count)
                .Select(x => x.Value)
                .OrderBy(x => EF.Functions.Random())
                .FirstOrDefaultAsync() ?? $"ERROR ({resource} : {count})", filteredArguments);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ConceptSignature>(coceptSignature =>
            {
                coceptSignature.HasOne(x => x.Article);
                coceptSignature.HasOne(x => x.Substantive);

                coceptSignature
                    .HasMany(x => x.Adjectives)
                    .WithMany(x => x.Signatures);
            });

            if (Database.IsSqlite())
            {
                modelBuilder
                    .Entity<Player>()
                    .Property(x => x.UserId)
                    .UseCollation(Collations.SqliteIgnoreCase);

                modelBuilder
                    .Entity<Token>()
                    .Property(x => x.Value)
                    .UseCollation(Collations.SqliteIgnoreCase);
            }
        }

        public async Task<IToken> CreateTokenAsync(string value)
        {
            return (await Tokens.SingleOrDefaultAsync(x => x.Value == value)) ?? new Token()
            {
                Value = value
            };
        }
    }
}
