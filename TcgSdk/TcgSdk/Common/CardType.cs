using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcgSdk.Common
{
    /// <summary>
    /// Supported card types
    /// </summary>
    public enum ITcgCardType
    {
        /// <summary>
        /// Pokemon cards
        /// </summary>
        Pokemon = 0,
        /// <summary>
        /// Magic the Gathering cards
        /// </summary>
        MagicTheGathering = 1
    }
}
