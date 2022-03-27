// Ishan Pranav's REBUS: WritingState.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

namespace Rebus
{
    /// <summary>
    /// Specifies the writing states of an expression.
    /// </summary>
    public enum WritingState
    {
        /// <summary>
        /// A sentence has not yet begun.
        /// </summary>
        Initial,

        /// <summary>
        /// A sentence is being written.
        /// </summary>
        Sentence,

        /// <summary>
        /// A white-space character has been written.
        /// </summary>
        WhiteSpace,

        /// <summary>
        /// A full-stop character has been written.
        /// </summary>
        FullStop
    }
}
