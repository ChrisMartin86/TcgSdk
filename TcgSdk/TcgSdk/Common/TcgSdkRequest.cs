using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace TcgSdk.Common
{
    internal class TcgSdkRequest<T>
    {
        private string method = "GET";
        private IDictionary<string, string> singleValueParametersDict = new Dictionary<string, string>();
        private IDictionary<string, string[]> multiValueParametersDict = new Dictionary<string, string[]>();
        private int pageSize = 100;
        private int pageNumber = 1;

        /// <summary>
        /// The Base URL of the api request, without filters applied, like https://api.pokemontcg.io/v1/cards
        /// </summary>
        public string BaseUrl { get; set; }
        /// <summary>
        /// The parameters of the request.
        /// </summary>
        public IEnumerable<TcgSdkRequestParameter> Parameters { get; set; }
        /// <summary>
        /// The method to use. Usually "GET"
        /// </summary>
        public string Method { get { return method; } set { method = value; } }
        /// <summary>
        /// Page size to return. Will throw ArgumentOutOfRangeException if value is greater than 1000.
        /// </summary>
        public int PageSize { get { return pageSize; } set { pageSize = checkPageSize(value); } }
        /// <summary>
        /// The page number to return. Defaults to 1.
        /// </summary>
        public int PageNumber { get { return pageNumber; } set { pageNumber = value; } }
        /// <summary>
        /// Get the deserialized response from the API.
        /// </summary>
        /// <returns>An ITcgCardResponse containing the deserialized cards.</returns>
        public TcgSdkResponse<T> GetResponse()
        {
            try
            {
                return JsonConvert.DeserializeObject<TcgSdkResponse<T>>(getHttpResponseString(buildRequestUrl(), "GET"));
            }
            catch (TcgSdkResponse<T>.TcgSdkResponseException e)
            {
                throw e;
            }
            catch (ITcgCardResponseDeserializationException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new Exception("There was a problem deserializing the response.", e);
            }
        }

        public static TcgSdkResponse<T> GetResponse(string baseUrl, IEnumerable<TcgSdkRequestParameter> parameters, string method)
        {
            var request = new TcgSdkRequest<T>
            {
                BaseUrl = baseUrl,
                Parameters = parameters,
                Method = method
            };

            return request.GetResponse();
        }
        /// <summary>
        /// Get all invalid parameters, or none if the object is ready to build the url
        /// </summary>
        /// <returns>Any invalid parameters, or none if the object is ready to build the url</returns>
        private IEnumerable<TcgSdkRequestParameter> getInvalidParameters()
        {
            var invalidParameters = new List<TcgSdkRequestParameter>();

            foreach (TcgSdkRequestParameter item in Parameters)
            {
                if (!item.Confirmed)
                {
                    invalidParameters.Add(item);
                }
            }

            return invalidParameters;
        }
        /// <summary>
        /// Build the full request URL from the base url (URL) and the parameter dictionary (Parameters).
        /// </summary>
        /// <returns>string containing full URL for request.</returns>
        private string buildRequestUrl()
        {
            StringBuilder urlSb = new StringBuilder(BaseUrl);

            if (null != Parameters)
            {
                if (BaseUrl.Substring(BaseUrl.Length - 1, 1) != "?")
                {
                    urlSb.Append("?");
                }

                foreach (TcgSdkRequestParameter item in Parameters)
                {
                    urlSb.Append(item.ToString());
                }
            }

            return urlSb.ToString().TrimEnd('&');
        }
        /// <summary>
        /// Ensure the requested page size is less than 1000
        /// </summary>
        /// <param name="value">The requested page size</param>
        /// <returns>The input value if it is less than 1000, otherwise will throw </returns>
        private int checkPageSize(int value)
        {
            if (value > 1000)
            {
                throw new ArgumentOutOfRangeException("Pagesize should not be greater than 1000");
            }
            else
            {
                return value;
            }
        }
        /// <summary>
        /// Get the JSON response string from the requested URL
        /// </summary>
        /// <param name="url">The URL to make the request to</param>
        /// <param name="method">The http method to use</param>
        /// <returns>JSON response</returns>
        private static string getHttpResponseString(string url, string method)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);

                webRequest.Method = method;

                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }

                    }
                }
            }
            catch (Exception e)
            {
                throw new TcgSdkResponse<T>.TcgSdkResponseException(e);
            }
        }

        public class ITcgCardResponseDeserializationException : Exception
        {
            public ITcgCardResponseDeserializationException() : base("There was a problem deserializing your request.")
            {

            }
        }

    }
}
//Param                 AcceptsList     Description
//name 	                true 	        The card name.
//nationalPokedexNumber false 	        The national pokedex number for a card that features a Pokémon supertype
//types                 true {and/or} 	The types of the card.These typically appear in the top right of card, and are denoted by energy symbol(ex.Fire, Fighting, Psychic, etc.)
//subtype 	            true 	        The subtype of the card.Examples include MEGA, Stage 1, BREAK, Supporter, etc.
//supertype 	        true 	        The supertype of the card.Either Pokémon, Trainer, or Energy.
//hp 	                false 	        The hit points of the card.This typically appears in the top right corner of the card.
//number                true 	        The number of the card for the set it was released in. Found on the bottom right side of the card.
//artist                true 	        The artist of the card.
//rarity                true 	        The rarity of the card (ex.Rare, Rare Holo, Common, etc.)
//series 	            true 	        The series the card appears in (ex.Base, XY, EX, etc.)
//set 	                true 	        The set the card appears in (ex.BREAKthrough, Phantom Forces, Jungle, etc.)
//setCode 	            true 	        The unique code of the set(ex.base1, xy5, ex3)
//retreatCost 	        false 	        The amount of energy it takes to retreat.Found on the bottom of the card.
//text                  true {and/or}   Any additional text on a card.This includes special rules (like MEGA rules or EX), and text that appears on Trainer cards.
//attackDamage 	        false 	        The attack damage of any given attack for a card
//attackCost 	        false 	        The energy cost of a given attack for a card
//weaknesses 	        true {and/or} 	The weaknesses of the card(ex.Fire, Water, Grass)
//resistances 	        true {and/or} 	The resistances of the card(ex.Fire, Water, Grass)
//layout                true 	        The card layout.Possible values: normal, split, flip, double-faced, token, plane, scheme, phenomenon, leveler, vanguard
//cmc 	                false 	        Converted mana cost.Always a number.
//colors                true {and/or}   The card colors.Usually this is derived from the casting cost, but some cards are special (like the back of dual sided cards and Ghostfire).
//type 	                true 	        The card type. This is the type you would see on the card if printed today. Note: The dash is a UTF8 'long dash’ as per the MTG rules
//supertypes 	        true {and/or}   The supertypes of the card. These appear to the far left of the card type. Example values: Basic, Legendary, Snow, World, Ongoing
//types 	            true {and/or}   The types of the card. These appear to the left of the dash in a card type. Example values: Instant, Sorcery, Artifact, Creature, Enchantment, Land, Planeswalker
//subtypes 	            true {and/or}   The subtypes of the card. These appear to the right of the dash in a card type. Usually each word is its own subtype. Example values: Trap, Arcane, Equipment, Aura, Human, Rat, Squirrel, etc.
//rarity 	            true 	        The rarity of the card. Examples: Common, Uncommon, Rare, Mythic Rare, Special, Basic Land
//set                   true            The set the card belongs to (set code).
//setName               true            The set the card belongs to.
//text                  true            The oracle text of the card. May contain mana symbols and other symbols.
//flavor                true            The flavor text of the card.
//power                 false           The power of the card.This is only present for creatures.This is a string, not an integer, because some cards have powers like: “1 + *”
//toughness             false           The toughness of the card.This is only present for creatures.This is a string, not an integer, because some cards have toughness like: “1 + *”
//loyalty               false           The loyalty of the card.This is only present for planeswalkers.
//foreignName           false           The name of a card in a foreign language it was printed in
//language              false           The language the card is printed in. Use this parameter when searching by foreignName
//gameFormat            false           The game format, such as Commander, Standard, Legacy, etc. (when used, legality defaults to Legal unless supplied)
//legality              false           The legality of the card for a given format, such as Legal, Banned or Restricted.
//page                  false           The page of data to request
//pageSize              false           The amount of data to return in a single request.The default is 100, the max is 1000.If more than 1000 is requested, 100 will be returned.