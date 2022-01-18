// Ishan Pranav's REBUS: IPathProvider.cs
// Copyright (c) Ishan Pranav. All Rights Reserved.
// Licensed under the MIT License.

using System;
using System.IO;

namespace Rebus.Server
{
    internal class PathProvider
    {
        private readonly string _directory;
        private readonly TypeFormatProvider _typeFormatProvider = new TypeFormatProvider();

        public PathProvider(string directory)
        {
            this._directory = directory;
        }

        public string GetPath(Type type, string? extension)
        {
            return Path.Combine(this._directory, String.Format(this._typeFormatProvider, format: "{0:f}.{1}", type, extension));
        }
    }
}
