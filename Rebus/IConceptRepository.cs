// Ishan Pranav's REBUS: IConceptRepository.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rebus
{
    public interface IConceptRepository
    {
        Task<IConcept> GetConceptAsync(int id);
        Task<IReadOnlyCollection<IConcept>> GetVisibleContentsAsync(int containerId, int viewerId);
    }
}
