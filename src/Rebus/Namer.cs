// Ishan Pranav's REBUS: Namer.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Text;

namespace Rebus
{
    public class Namer : INamer
    {
        private readonly string[] _stars = new string[]
        {
            "Alpha",
            "Beta",
            "Gamma",
            "Delta",
            "Epsilon",
            "Zeta",
            "Eta",
            "Theta",
            "Iota",
            "Kappa",
            "Lambda",
            "Mu",
            "Nu",
            "Ksi",
            "Omicron",
            "Pi",
            "Rho",
            "Sigma",
            "Tau",
            "Upsilon",
            "Phi",
            "Psi",
            "Chi",
            "Omega"
        };
        private readonly string[] _constellations = new string[]
        {
            "Andromedae",
            "Antliae",
            "Api",
            "Aquarii",
            "Aquilae",
            "Arae",
            "Aries",
            "Aurigae",
            "Bootetis",
            "Caeli",
            "Camelopardalis",
            "Cancri",
            "Canes Venaticiorum",
            "Canis Majoris",
            "Canis Minoris",
            "Capricorni",
            "Carinae",
            "Cassiopeiae",
            "Centauri",
            "Cephei",
            "Ceti",
            "Chamaelei",
            "Circini",
            "Columbae",
            "Comae Berenices",
            "Coronae Australis",
            "Coronae Borealis",
            "Corvi",
            "Crater",
            "Crucis",
            "Cygni",
            "Delphini",
            "Doradonis",
            "Draconis",
            "Equulei",
            "Eridani",
            "Fornacis",
            "Geminorum",
            "Gri",
            "Hercules",
            "Horologii",
            "Hydrae",
            "Hydri",
            "Indi",
            "Lacertae",
            "Leonis",
            "Leonis Minoris",
            "Leporis",
            "Librae",
            "Lupi",
            "Lyncis",
            "Lyrae",
            "Mensae",
            "Microscopii",
            "Monoceros",
            "Muscae",
            "Normae",
            "Octantis",
            "Ophiuchi",
            "Orionis",
            "Pavonis",
            "Pegasi",
            "Persei",
            "Phoenicis",
            "Pictoris",
            "Pisces",
            "Piscis Austrini",
            "Puppis",
            "Pyxis",
            "Reticuli",
            "Sagittae",
            "Sagittarii",
            "Scorpii",
            "Sculptoris",
            "Scuti",
            "Serpentis",
            "Sextantis",
            "Tauri",
            "Telescopii",
            "Trianguli",
            "Trianguli Australis",
            "Tucanae",
            "Ursae Majoris",
            "Ursae Minoris",
            "Velae",
            "Virginis",
            "Volantis",
            "Vulpeculae"
        };
        private int _constellation = 0;
        private int _star = 0;
        private int _planet = 0;

        public string Name(int degree)
        {
            string result = new StringBuilder(_stars[_star])
                .Append(' ')
                .Append(_constellations[_constellation])
                .Append(' ')
                .Append(_planet + 1)
                .ToString();

            _planet++;

            if (_planet == 10)
            {
                _planet = 0;
                _star++;
            }

            if (_star == _stars.Length)
            {
                _star = 0;
                _constellation++;
            }

            return result;
        }
    }
}
