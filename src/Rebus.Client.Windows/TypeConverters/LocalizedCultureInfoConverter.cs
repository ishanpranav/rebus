// Ishan Pranav's REBUS: LocalizedCultureInfoConverter.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace Rebus.Client.Windows.TypeConverters
{
    internal sealed class LocalizedCultureInfoConverter : CultureInfoConverter
    {
        private readonly StandardValuesCollection _standardValues;

        public LocalizedCultureInfoConverter()
        {
            List<CultureInfo> cultures = new List<CultureInfo>();

            foreach (string directory in Directory.GetDirectories(Directory.GetCurrentDirectory()))
            {
                try
                {
                    cultures.Add(new CultureInfo(new DirectoryInfo(directory).Name));
                }
                catch (CultureNotFoundException)
                {

                }
            }

            _standardValues = new StandardValuesCollection(cultures);
        }

        protected override string GetCultureName(CultureInfo culture)
        {
            return culture.DisplayName;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
        {
            return _standardValues;
        }
    }
}
