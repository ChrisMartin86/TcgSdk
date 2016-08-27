using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcgSdk.Common
{
    /// <summary>
    /// Supported response types
    /// </summary>
    public enum TcgSdkResponseType
    {
        /// <summary>
        /// Pokemon cards
        /// </summary>
        PokemonCard = 0,
        /// <summary>
        /// Magic the Gathering cards
        /// </summary>
        MagicCard = 1,
        /// <summary>
        /// Pokemon TCG sets
        /// </summary>
        PokemonSet = 2,
        /// <summary>
        /// Magic TCG sets
        /// </summary>
        MagicSet = 3
    }
}
