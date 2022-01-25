// Ishan Pranav's REBUS: FileRepository.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.IO;

namespace Rebus.Server
{
    internal sealed class FileRepository
    {
        private readonly string _directory;
        private readonly TypeFormatProvider _typeFormatProvider = new TypeFormatProvider();

        public FileRepository(string directory)
        {
            _directory = directory;
        }

        public string GetPath(Type type, string extension)
        {
            return Path.Combine(_directory, string.Format(_typeFormatProvider, format: "{0:f}.{1}", type, extension));
        }
    }
}
