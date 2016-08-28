using System;
using TcgSdk.Common;
using System.Collections.Generic;

namespace TcgSdk.Pokemon
{
    /// <summary>
    /// Pokemon card set from https://api.pokemontcg.io
    /// </summary>
    public class PokemonSet : ITcgSdkObject
    {
        /// <summary>
        /// The set code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// The set name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The set series
        /// </summary>
        public string Series { get; set; }
        /// <summary>
        /// Total cards in the set
        /// </summary>
        public int TotalCards { get; set; }
        /// <summary>
        /// True/False if the set is standard legal
        /// </summary>
        public bool StandardLegal { get; set; }
        /// <summary>
        /// Set release date
        /// </summary>
        public DateTime ReleaseDate { get; set; }
        /// <summary>
        /// The type of response. Always returns TcgSdkResponseType.PokemonSet for this class.
        /// </summary>
        public TcgSdkResponseType ResponseType { get { return TcgSdkResponseType.PokemonSet; } }

        /// <summary>
        /// Keep the constructor internal.
        /// </summary>
        internal PokemonSet()
        {

        }

        public override string ToString()
        {
            return Name;
        }

        public IEnumerable<PokemonCard> GetCardsInSet()
        {
            try
            {
                var requestParameters = new TcgSdkRequestParameter("set", Code, false, false);

                var cards = ITcgSdkResponseFactory<PokemonCard>.Get(TcgSdkResponseType.PokemonCard, new TcgSdkRequestParameter[] { requestParameters });

                return cards.Cards;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
