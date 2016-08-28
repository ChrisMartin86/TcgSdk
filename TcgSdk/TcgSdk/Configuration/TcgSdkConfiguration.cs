using System;
using TcgSdk.Common;

namespace TcgSdk.Configuration
{
    /// <summary>
    /// Configuration methods and data from TcgSdk. Ideally would come from a config file.
    /// </summary>
    public static class TcgSdkConfiguration
    {
        public static string GetApiBaseAddress(string endPoint, TcgSdkResponseType responseType)
        {
            switch (responseType)
            {
                case TcgSdkResponseType.PokemonCard:
                    return "https://api.pokemontcg.io/v1/" + endPoint;
                case TcgSdkResponseType.MagicCard:
                    return "https://api.magicthegathering.io/v1/" + endPoint;
                case TcgSdkResponseType.PokemonSet:
                    return "https://api.pokemontcg.io/v1/" + endPoint;
                case TcgSdkResponseType.MagicSet:
                    return "https://api.magicthegathering.io/v1/" + endPoint;
                default:
                    throw new NotImplementedException(string.Format("TcgSdkResponseType {0} not supported", responseType.ToString()));
            }   
        }
    }
}