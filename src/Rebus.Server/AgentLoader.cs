﻿// Ishan Pranav's REBUS: AgentLoader.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Xml;
using System.Xml.Serialization;
using Rebus.Server.Considerations;

namespace Rebus.Server
{
    internal sealed class AgentLoader
    {
        private readonly string _connectionString;
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(Agent));

        public AgentLoader(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Agent Load()
        {
            using (XmlReader xmlReader = XmlReader.Create(_connectionString, new XmlReaderSettings()
            {
                XmlResolver = null
            }))
            {
                if (_xmlSerializer.Deserialize(xmlReader) is Agent result)
                {
                    return result;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
