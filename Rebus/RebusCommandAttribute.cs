// Ishan Pranav's REBUS: RebusCommandAttribute.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;

namespace Rebus
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class RebusCommandAttribute : Attribute
    {
        public string Verb { get; }
        public string? Adverb { get; }

        public bool Reflexive { get; set; }
        public Characteristics DirectObject { get; set; }

        public RebusCommandAttribute(string verb)
        {
            this.Verb = verb;
        }

        public RebusCommandAttribute(string verb, string adverb) : this(verb)
        {
            this.Adverb = adverb;
        }

        public bool Matches(IToken verb, IToken? adverb, bool reflexive, Characteristics directObject)
        {
            return this.Verb.Equals(verb.Value, StringComparison.OrdinalIgnoreCase) && ((this.Adverb is null && adverb is null) || (this.Adverb is not null && adverb is not null && this.Adverb.Equals(adverb.Value, StringComparison.OrdinalIgnoreCase))) && this.Reflexive == reflexive && this.DirectObject.HasFlag(directObject);
        }
    }
}
