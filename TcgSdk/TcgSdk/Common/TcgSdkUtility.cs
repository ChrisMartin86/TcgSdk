using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TcgSdk.Common.Cards;

namespace TcgSdk.Common
{
    public static class TcgSdkUtility
    {
        private static Random random = new Random();

        /// <summary>
        /// Get a random integer with the static random from this utility.
        /// </summary>
        /// <param name="inclusiveStartBoundary">The inclusive start boundary for the range. This number can possibly be returned as a result.</param>
        /// <param name="exclusiveEndBoundary">The exclusive end boundary for the range. This number cannot be returned as a result.</param>
        /// <returns>Random number within the range of numbers provided.</returns>
        internal static int GetRandomInt(int inclusiveStartBoundary, int exclusiveEndBoundary)
        {
            return random.Next(inclusiveStartBoundary, exclusiveEndBoundary);
        }

        /// <summary>
        /// Roll a number of dice.
        /// </summary>
        /// <param name="numberOfSides">The number of sides each die should have.</param>
        /// <param name="numberOfDice">The number of dice to roll.</param>
        /// <returns>An array of the results. Get the sum for the total.</returns>
        public static IEnumerable<int> RollDice(int numberOfSides, int numberOfDice)
        {
            List<int> resultArray = new List<int>();

            for (int i = 0; i < numberOfDice; i++)
            {
                resultArray.Add(GetRandomInt(1, (numberOfSides + 1)));
            }

            return resultArray;
        }
        /// <summary>
        /// Roll a number of dice asynchronously. Might be faster if you are rolling a truely huge number of dice, but otherwise just for fun.
        /// </summary>
        /// <param name="numberOfSides">The number of sides each die should have.</param>
        /// <param name="numberOfDice">The number of dice to roll.</param>
        /// <returns>Awaitable task containing an array of the results. Get the sum for the total.</returns>
        public async static Task<IEnumerable<int>> RollDiceAsync(int numberOfSides, int numberOfDice)
        {
            List<Task<IEnumerable<int>>> taskList = null;

            List<int> resultArray = new List<int>();

            for (int i = 0; i < numberOfDice; i++)
            {
                taskList.Add(Task.Factory.StartNew(() => RollDice(numberOfSides, 1)));
            }

            foreach (Task<IEnumerable<int>> item in taskList)
            {
                IEnumerable<int> result = await item;

                resultArray.Concat(result);
            }

            return resultArray;


        }

        public static CoinFlipResult FlipCoin()
        {
            return (CoinFlipResult)GetRandomInt(0, 2);
        }

        /// <summary>
        /// Coin flip results enum. 
        /// </summary>
        public enum CoinFlipResult
        {
            /// <summary>
            /// Result is heads
            /// </summary>
            Heads = 0,
            /// <summary>
            /// Result is tails
            /// </summary>
            Tails = 1
        }

        /// <summary>
        /// Turn an Dictionary ITcgCard, int into an List of ITcgCards with one copy of each ITcgCard per associated int value.
        /// </summary>
        /// <param name="dict">The dictionary of cards and card numbers.</param>
        /// <returns>List of ITcgCards</returns>
        internal static List<ITcgCard> dictToList(IDictionary<ITcgCard, int> dict)
        {
            var deckList = new List<ITcgCard>();

            foreach (KeyValuePair<ITcgCard, int> item in dict)
            {
                for (int i = 0; i < item.Value; i++)
                {
                    deckList.Add(item.Key);
                }
            }

            return deckList;
        }
        /// <summary>
        /// Turn a List of ITcgCards into a Dictionary ITcgCard, int with the int value incremented by one for each copy of the card found in the list.
        /// </summary>
        /// <param name="dict">The list of cards.</param>
        /// <returns>Dictionary ITcgCard, int of ITcgCards</returns>
        internal static Dictionary<ITcgCard, int> listToDict(List<ITcgCard> list)
        {
            var dict = new Dictionary<ITcgCard, int>();

            foreach (ITcgCard item in list)
            {
                try
                {
                    dict.Add(item, 1);
                }
                catch
                {
                    try
                    {
                        dict[item]++;
                    }
                    catch (Exception e)
                    {
                        // Serious problem with my logic, this shouldn't get hit ever, but just in case.
                        throw new Exception("Your developer has made a grave mistake.", e);
                    }
                }
            }

            return dict;
        }

        /// <summary>
        /// Get the JSON response string from the requested URL
        /// </summary>
        /// <param name="url">The URL to make the request to</param>
        /// <param name="method">The http method to use</param>
        /// <returns>JSON response</returns>
        internal static string getHttpResponseString(string url, string method)
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

        /// <summary>
        /// Get the int count from an IDictionary ITcgCard, int representing the cards.
        /// </summary>
        /// <param name="cards">The IDictionary ITcgCard, int representing the cards.</param>
        /// <returns>The total number of cards in the set.</returns>
        internal static int getCount(IDictionary<ITcgCard, int> cards)
        {
            int count = 0;

            foreach (KeyValuePair<ITcgCard, int> item in cards)
            {
                for (int i = 0; i < item.Value; i++)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
