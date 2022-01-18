// Ishan Pranav's REBUS: 
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

namespace Rebus.Server.Console
{
    internal class ConsoleExpressionWriter : ExpressionWriter
    {
        protected override void WriteCore(char value)
        {
            System.Console.Write(value);
        }

        protected override void WriteLineCore()
        {
            System.Console.WriteLine();
        }
    }
}