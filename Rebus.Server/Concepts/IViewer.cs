// Ishan Pranav's REBUS: IViewer.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus.Server.Concepts
{
    internal interface IViewer : IWritable
    {
        HexPoint Region { get; }

        Task ViewAsync(ExpressionWriter writer, RebusDbContext context);
    }
}
