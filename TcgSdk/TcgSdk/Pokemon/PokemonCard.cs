using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using TcgSdk.System;
using System.Text.RegularExpressions;


namespace TcgSdk.Pokemon
{
    public class PokemonCard : ITcgCard
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
        /// The type of card. Always returns CardType.Pokemon for this class.
        /// </summary>
        public CardType CardType { get { return CardType.Pokemon; } }
        /// <summary>
        /// The national pokedex number for a card that features a Pokémon supertype
        /// </summary>
        public int? NationalPokedexNumber { get; set; }
        /// <summary>
        /// The types of the card. These typically appear in the top right of card, and are denoted by energy symbol (ex. Fire, Fighting, Psychic, etc.)
        /// </summary>
        public string[] Types { get; set; }
        /// <summary>
        ///	The subtype of the card. Examples include MEGA, Stage 1, BREAK, Supporter, etc.
        /// </summary>
        public string SubType { get; set; }
        /// <summary>
        /// The supertype of the card. Either Pokémon, Trainer, or Energy.
        /// </summary>
        public string SuperType { get; set; }
        /// <summary>
        /// The hit points of the card. This typically appears in the top right corner of the card.
        /// </summary>
        public string HP { get; set; }
        /// <summary>
        /// The number of the card for the set it was released in. Found on the bottom right side of the card.
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// The artist of the card.
        /// </summary>
        public string Artist { get; set; }
        /// <summary>
        /// The rarity of the card (ex. Rare, Rare Holo, Common, etc.)
        /// </summary>
        public string Rarity { get; set; }
        /// <summary>
        /// The series the card appears in (ex. Base, XY, EX, etc.)
        /// </summary>
        public string Series { get; set; }
        /// <summary>
        ///	The set the card appears in (ex. BREAKthrough, Phantom Forces, Jungle, etc.)
        /// </summary>
        public string Set { get; set; }
        /// <summary>
        ///	The unique code of the set (ex. base1, xy5, ex3)
        /// </summary>
        public string SetCode { get; set; }
        /// <summary>
        /// The amount of energy it takes to retreat. Found on the bottom of the card.
        /// </summary>
        public string[] RetreatCost { get; set; }
        /// <summary>
        /// Any additional text on a card. This includes special rules (like MEGA rules or EX), and text that appears on Trainer cards.
        /// </summary>
        public string[] Text { get; set; }
        /// <summary>
        /// The Pokemon's attacks
        /// </summary>
        public Attack[] Attacks { get; set; }
        /// <summary>
        /// The weaknesses of the card (ex. Fire, Water, Grass)
        /// </summary>
        public Weakness[] Weaknesses { get; set; }
        /// <summary>
        /// The resistances of the card (ex. Fire, Water, Grass)
        /// </summary>
        public Resistance[] Resistances { get; set; }
        /// <summary>
        /// A unique id for this card. It is made up by taking the set code and concatenating the card number to it. (ex. xy1-1)
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Get cards from the API using a URL filter.
        /// </summary>
        /// <param name="filter">The URL filter. Null is acceptable.</param>
        /// <returns>PokemonCardResponse with the requested cards.</returns>
        public static IEnumerable<PokemonCard> Get(string filter = null)
        {
            if (!validateFilter(filter))
                throw new ArgumentException(string.Format("Filter {0} is badly formed.", filter ?? null));

            string responseJson;

            try
            {
                responseJson = getResponse(filter);
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                return (JsonConvert.DeserializeObject<PokemonCardResponse>(responseJson)).Cards;
            }
            catch (Exception e)
            {
                throw new Exception("There was a problem deserializing the response.", e);
            }

        }
        /// <summary>
        /// Get the response JSON from the API using a URL filter.
        /// </summary>
        /// <param name="filter">The URL filter. Null is acceptable.</param>
        /// <returns>JSON Response string</returns>
        private static string getResponse(string filter = null)
        {
            if (string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter))
                filter = null;

            string url = "https://api.pokemontcg.io/v1/cards/" + filter;

            try
            {
                return ApiCommunicator.GetHttpResponseString(url, "GET");
            }
            catch (Exception e)
            {
                throw new Exception("There was a problem getting a response from the API.", e);
            }
        }
        /// <summary>
        /// Validate that the filter is formatted well. Will check for ? at the beginning, and a match to the RegEx [\s\w]+[=][\s\w]+.
        /// </summary>
        /// <param name="input">The input to validate.</param>
        /// <returns>TRUE/FALSE if the input is valid.</returns>
        private static bool validateFilter(string input)
        {
            bool validated = true;

            if (null == input)
                return validated;

            if (!Regex.IsMatch(input, @"[\s\w]+[=][\s\w]+"))
                validated = false;

            if (input.Substring(0,1) != "?")
                validated = false;

            return validated;
        }

        private struct PokemonCardResponse
        {
            public PokemonCard[] Cards { get; set; }
        }

        public struct Attack
        {
            public int ConvertedEnergyCost { get; set; }
            public string[] Cost { get; set; }
            public string Damage { get; set; }
            public string Name { get; set; }
            public string Text { get; set; }
        }

        public struct Weakness
        {
            public string Type { get; set; }
            public string Value { get; set; }
        }

        public struct Resistance
        {
            public string Type { get; set; }
            public string Value { get; set; }
        }

    }
}
