using System;
using System.Collections.Generic;

namespace TcgSdk.Common
{
    /// <summary>
    /// Common code for creating/retrieving cards from the supported APIs
    /// </summary>
    /// <typeparam name="T">The type of card to create/retrieve</typeparam>
    public class ITcgCardFactory<T>
    {
        /// <summary>
        /// Get the cards of the requested type
        /// </summary>
        /// <param name="cardType">CardType. Should correspond with T.</param>
        /// <param name="filters">Filter parameters</param>
        /// <returns>IEnumerable of requested cards</returns>
        public static IEnumerable<T> Get(ITcgCardType cardType, IDictionary<string, string> filters)
        {
            try
            {
                var request = new ITcgCardRequest<T>
                {
                    Url = getBaseUrl(cardType),
                    Parameters = filters,
                    Method = "GET"
                };

                return (request.GetResponse()).Cards;

            }
            catch (Exception e)
            {
                throw new Exception("There was a problem getting the requested cards", e);
            }
        }
        /// <summary>
        /// Method to return base url
        /// </summary>
        /// <param name="cardtype">The type of card being requested</param>
        /// <returns>The base url</returns>
        private static string getBaseUrl(ITcgCardType cardtype)
        {
            switch (cardtype)
            {
                case ITcgCardType.Pokemon:
                    return "https://api.pokemontcg.io/v1/cards";
                case ITcgCardType.MagicTheGathering:
                    return "https://api.magicthegathering.io/v1/cards";
                default:
                    throw new ArgumentException(string.Format("CardType {0} not supported", cardtype));
            }
            
        }
    }
}
