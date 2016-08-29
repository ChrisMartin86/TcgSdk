using System;
using System.Collections.Generic;
using TcgSdk.Configuration;

namespace TcgSdk.Common
{
    /// <summary>
    /// Common code for creating/retrieving cards from the supported APIs
    /// </summary>
    /// <typeparam name="T">The type of card to create/retrieve</typeparam>
    public class ITcgSdkResponseFactory<T>
    {
        /// <summary>
        /// Get the response of the requested type
        /// </summary>
        /// <param name="responseType">TcgSdkResponseType. Should correspond with T.</param>
        /// <param name="parameters">Filter parameters</param>
        /// <returns>IEnumerable of requested cards</returns>
        public static ITcgSdkResponse<T> Get(TcgSdkResponseType responseType, IEnumerable<TcgSdkRequestParameter> parameters, int pageNumber = 1, int pageSize = 100)
        {
            string baseUrl;

            try
            {
                baseUrl = getBaseUrl(responseType);
            }
            catch (NotImplementedException e)
            {
                TcgSdkErrorLog.WriteLog(e, System.Diagnostics.EventLogEntryType.Error, 3);
                throw;
            }
            catch (Exception e)
            {
                TcgSdkErrorLog.WriteLog(e, System.Diagnostics.EventLogEntryType.Error, 0);
                throw;
            }

            try
            {
                return TcgSdkRequest<T>.GetResponse(baseUrl, parameters, "GET", pageNumber, pageSize);
            }
            catch (TcgSdkResponse<T>.TcgSdkResponseException e)
            {
                TcgSdkErrorLog.WriteLog(e, System.Diagnostics.EventLogEntryType.Error, 1);
                throw;
            }
            catch (TcgSdkRequest<T>.ITcgCardResponseDeserializationException e)
            {
                TcgSdkErrorLog.WriteLog(e, System.Diagnostics.EventLogEntryType.Error, 2);
                throw;
            }
            catch (Exception e)
            {
                TcgSdkErrorLog.WriteLog(e, System.Diagnostics.EventLogEntryType.Error, 0);
                throw;
            }
        }


        private static string getBaseUrl(TcgSdkResponseType responseType)
        {
            string baseUrl;

            try
            {
                switch (responseType)
                {
                    case TcgSdkResponseType.PokemonCard:
                        {
                            baseUrl = TcgSdkConfiguration.GetApiBaseAddress("cards", responseType);
                            break;
                        }
                    case TcgSdkResponseType.MagicCard:
                        {
                            baseUrl = TcgSdkConfiguration.GetApiBaseAddress("cards", responseType);
                            break;
                        }
                    case TcgSdkResponseType.PokemonSet:
                        {
                            baseUrl = TcgSdkConfiguration.GetApiBaseAddress("sets", responseType);
                            break;
                        }
                    case TcgSdkResponseType.MagicSet:
                        {
                            baseUrl = TcgSdkConfiguration.GetApiBaseAddress("sets", responseType);
                            break;
                        }
                    default:
                        {
                            throw new NotImplementedException(string.Format("TcgSdkResponseType {0} is not supported", responseType.ToString()));
                        }
                }

                return baseUrl;

            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Turn an Dictionary T, int into an List of T with one copy of each T per associated int value.
        /// </summary>
        /// <param name="dict">The dictionary of T and number of each T in list.</param>
        /// <returns>List of ITcgCards</returns>
        private static List<T> dictToList(IDictionary<T, int> dict)
        {
            var deckList = new List<T>();

            foreach (KeyValuePair<T, int> item in dict)
            {
                for (int i = 0; i < item.Value; i++)
                {
                    deckList.Add(item.Key);
                }
            }

            return deckList;
        }

        /// <summary>
        /// Turn a List of T into a Dictionary T, int with the int value incremented by one for each copy of the T found in the list.
        /// </summary>
        /// <param name="dict">The list of T.</param>
        /// <returns>Dictionary T, int of T</returns>
        private static Dictionary<T, int> listToDict(IEnumerable<T> list)
        {
            var dict = new Dictionary<T, int>();

            foreach (T item in list)
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
    }
}
