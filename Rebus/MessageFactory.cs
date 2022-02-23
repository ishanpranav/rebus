// Ishan Pranav's REBUS: MessageFactory.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Rebus
{
    public abstract class MessageFactory
    {
        public async Task<RebusException> CreateExceptionAsync(int resource, params object[] arguments)
        {
            return new RebusException(await CreateMessageAsync(resource, arguments));
        }

        public Task<IWritable> CreateMessageAsync(int resource, params object[] arguments)
        {
            return CreateMessageAsync(player: null, subject: null, resource, arguments);
        }

        public abstract Task<IWritable> CreateMessageAsync(IConcept? player, IConcept? subject, int resource, params object[] arguments);
    }
}
