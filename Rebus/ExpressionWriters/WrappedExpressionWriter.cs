// Ishan Pranav's REBUS: AnsiExpressionWriter.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System.Text;
using System.Text.RegularExpressions;
using Rebus.WritingStates;

namespace Rebus.ExpressionWriters
{
    public class WrappedExpressionWriter : StringExpressionWriter
    {
        public int Width { get; set; } = 80;

        public WrappedExpressionWriter() { }
        private protected WrappedExpressionWriter(IWritingState state) : base(state) { }

        /// <summary>Wraps the text contained in the writer.</summary>
        /// <remarks>The implementation of this method was inspired by and based on <see href="https://stackoverflow.com/questions/52605996/c-sharp-console-word-wrap">this</see> Stack Overflow question asked by <see href="https://stackoverflow.com/users/8853235/alasdair-c">Alasdair C</see> and the answer provided by <see href="https://stackoverflow.com/users/2224523/mike">Mike</see>. The software code is licensed under the Creative Commons <see href="https://creativecommons.org/licenses/by-sa/3.0/">Attribution-ShareAlike 3.0 Unported (CC BY-SA 3.0)</see> license.</remarks>
        /// <seealso href="https://stackoverflow.com/questions/52605996/c-sharp-console-word-wrap">C# Console Word Wrap - Stack Overflow</seealso>
        /// <seealso href="https://stackoverflow.com/users/8853235/alasdair-c">Alasdair C</seealso>
        /// <seealso href="https://stackoverflow.com/users/2224523/mike">Mike</seealso>
        /// <seealso href="https://creativecommons.org/licenses/by-sa/3.0/">Attribution-ShareAlike 3.0 Unported (CC BY-SA 3.0)</seealso>
        public void Wrap()
        {
            string text = StringBuilder.ToString();
            int index = 0;
            int length = text.Length;
            MatchCollection strongBreakMatches = Regex.Matches(text, pattern: @"\n", RegexOptions.Compiled);
            MatchCollection weakBreakMatches = Regex.Matches(text, pattern: @"\s+|(?<=[-,.;])|$", RegexOptions.Compiled);

            StringBuilder.Clear();

            while (index < length)
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
                    StringBuilder.AppendLine(text.Substring(index, Width));

                    index += Width;
                }
                else
                {
                    int matchIndex = match.Index;

                    StringBuilder.AppendLine(text.Substring(index, matchIndex - index));

                    index = matchIndex + match.Length;
                }
            }

            bool isFound(Match match)
            {
                return match.Index >= index && match.Index <= index + Width;
            }
        }

        public override ExpressionWriter BeginFragment()
        {
            return new WrappedExpressionWriter(new SentenceWritingState());
        }
    }
}
