// Ishan Pranav's REBUS: Argument.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    /// <summary>
    /// Specifies the noun argument of a verb.
    /// </summary>
    public enum Argument
    {
        /// <summary>
        /// No argument.
        /// </summary>
        None,

        /// <summary>
        /// A subject.
        /// </summary>
        Subject,

        /// <summary>
        /// A direct object.
        /// </summary>
        DirectObject,

        /// <summary>
        /// The indirect object.
        /// </summary>
        IndirectObject
    }
}
