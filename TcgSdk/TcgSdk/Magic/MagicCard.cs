using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcgSdk.Magic
{
    public class MagicCard : ITcgCard
    {
        /// <summary>
        /// The name of the card
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The URL to the image of the card
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// The type of card. Always returns CardType.MagicTheGathering for this class.
        /// </summary>
        public CardType CardType { get { return CardType.MagicTheGathering; } }
    }
}
