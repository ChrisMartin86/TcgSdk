using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcgSdk
{
    /// <summary>
    /// Common interface for all supported cards
    /// </summary>
    public interface ITcgCard
    {
        /// <summary>
        /// The name of the card
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// The URL to the image of the card
        /// </summary>
        string ImageUrl { get; set; }
        /// <summary>
        /// The type of card
        /// </summary>
        CardType CardType { get; }
    }
    /// <summary>
    /// Supported card types
    /// </summary>
    public enum CardType
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
