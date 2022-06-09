// Ishan Pranav's REBUS: Wrapper.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Text;
using System.Text.RegularExpressions;

namespace Rebus.Server.Tcp
{
    internal sealed class Wrapper
    {
        public int Width { get; set; } = 80;

        /// <summary>Wraps the text contained in the string builder.</summary>
        /// <remarks>
        /// The implementation of this method was inspired by and based on <see href="https://stackoverflow.com/questions/52605996/c-sharp-console-word-wrap">this</see> Stack Overflow question asked by <see href="https://stackoverflow.com/users/8853235/alasdair-c">Alasdair C</see> and the answer provided by <see href="https://stackoverflow.com/users/2224523/mike">Mike</see>. The software code is licensed under the Creative Commons <see href="https://creativecommons.org/licenses/by-sa/3.0/">Attribution-ShareAlike 3.0 Unported (CC BY-SA 3.0)</see> license.
        /// </remarks>
        /// <seealso href="https://stackoverflow.com/questions/52605996/c-sharp-console-word-wrap">C# Console Word Wrap - Stack Overflow</seealso>
        /// <seealso href="https://stackoverflow.com/users/8853235/alasdair-c">Alasdair C</seealso>
        /// <seealso href="https://stackoverflow.com/users/2224523/mike">Mike</seealso>
        /// <seealso href="https://creativecommons.org/licenses/by-sa/3.0/">Attribution-ShareAlike 3.0 Unported (CC BY-SA 3.0)</seealso>
        /// <param name="value">The string builder to wrap.</param>
        public void Wrap(StringBuilder value)
        {
            string text = value.ToString();
            int index = 0;
            MatchCollection strongBreakMatches = Regex.Matches(text, pattern: @"\n", RegexOptions.Compiled);
            MatchCollection weakBreakMatches = Regex.Matches(text, pattern: @"\s+|(?<=[-,.;])|$", RegexOptions.Compiled);

            value.Clear();

            while (index < text.Length)
            {
                Match? match = null;

                foreach (Match strongBreakMatch in strongBreakMatches)
                {
                    if (isFound(strongBreakMatch))
                    {
                        match = strongBreakMatch;

                        break;
                    }
                }

                if (match is null)
                {
                    for (int i = weakBreakMatches.Count - 1; i >= 0; i--)
                    {
                        Match weakBreakMatch = weakBreakMatches[i];

                        if (isFound(weakBreakMatch))
                        {
                            match = weakBreakMatch;

                            break;
                        }
                    }
                }

                if (match is null)
                {
                    value.AppendLine(text.Substring(index, Width));

                    index += Width;
                }
                else
                {
                    value.AppendLine(text.Substring(index, match.Index - index));

                    index = match.Index + match.Length;
                }
            }

            bool isFound(Match match)
            {
                return match.Index >= index && match.Index <= index + Width;
            }
        }
    }
}
