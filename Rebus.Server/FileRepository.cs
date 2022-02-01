// Ishan Pranav's REBUS: FileRepository.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Text;

namespace Rebus.Server
{
    internal sealed class FileRepository
    {
        private readonly string _directory;

        public FileRepository(string directory)
        {
            _directory = directory;
        }

        public string GetPath(Type type, string extension)
        {
            StringBuilder result = new StringBuilder(type.Namespace)
                .Append('.');

            format(type, result);

            result
                .Append('.')
                .Append(extension);

            return Path.Combine(_directory, result.ToString());

            static void format(Type value, StringBuilder stringBuilder)
            {
                string name = value.Name;
                int start = 0;

                if (value.IsInterface && name.StartsWith('I'))
                {
                    start = 1;
                }

                if (value.IsGenericType)
                {
                    foreach (Type genericArgument in value.GetGenericArguments())
                    {
                        format(genericArgument, stringBuilder);
                    }

                    stringBuilder.Append(name.AsSpan(start, name.IndexOf('`') - start));
                }
                else
                {
                    stringBuilder.Append(name);
                }
            }
        }
    }
}
