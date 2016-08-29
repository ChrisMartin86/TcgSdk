using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcgSdk;
using TcgSdk.Magic;
using TcgSdk.Pokemon;
using TcgSdk.Common;
using TcgSdk.Common.Cards;

namespace TcgSdkTestConsole
{
    class Program
    {
        static List<TcgSdkRequestParameter> parameters = new List<TcgSdkRequestParameter>();

        static void Main()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Would you like Pokemon or Magic?");
                string pokemonOrMagic = Console.ReadLine();
                Console.WriteLine("What set would you like to look for?");
                string setToSearchFor = Console.ReadLine();

                parameters.Add(new TcgSdkRequestParameter("name", setToSearchFor, false, false));

                if (pokemonOrMagic.ToUpper() == "POKEMON")
                {
                    ITcgSdkResponse<PokemonSet> pokemonSetResponse = ITcgSdkResponseFactory<PokemonSet>.Get(TcgSdkResponseType.PokemonSet, parameters);

                    PokemonSet[] setsInfo = pokemonSetResponse.Sets;

                    foreach (PokemonSet set in setsInfo)
                    {
                        Console.WriteLine(string.Format("Set name is {0}", set.Name));
                        Console.WriteLine("------------------------------------");

                        IEnumerable<PokemonCard> cardsInSet = set.GetCardsInSet();

                        foreach (PokemonCard card in cardsInSet)
                        {
                            Console.WriteLine(string.Format("{0} - {1}", card.Name, card.Rarity));

                            string types = string.Empty;

                            if (null != card.Types)
                            {
                                foreach (string type in card.Types)
                                {
                                    types = types + type + ",";
                                }

                                types = types.TrimEnd(',');
                            }

                            Console.WriteLine(string.Format("Type: {0}", types));
                            Console.WriteLine("------------------");
                        }

                        Console.ReadLine();
                    }
                }
                else if (pokemonOrMagic.ToUpper() == "MAGIC")
                {

                    ITcgSdkResponse<MagicSet> magicSetResponse = ITcgSdkResponseFactory<MagicSet>.Get(TcgSdkResponseType.MagicSet, parameters);

                    MagicSet[] setsInfo = magicSetResponse.Sets;

                    foreach (MagicSet set in setsInfo)
                    {
                        Console.WriteLine(string.Format("Set name is {0}", set.Name));
                        Console.WriteLine("------------------------------------");

                        IEnumerable<MagicCard> cardsInSet = set.GetCardsInSet();

                        foreach (MagicCard card in cardsInSet)
                        {
                            Console.WriteLine(string.Format("{0} - {1}", card.Name, card.Rarity));
                            Console.WriteLine(string.Format("Type: {0}", card.Type));
                            Console.WriteLine("------------------");
                        }

                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine("Unknown response! Use either Pokemon or Magic (not case sensitive).");
                }

                Console.WriteLine("Start over? Y/N");

                string response = Console.ReadLine();

                if (response.ToUpper() == "Y")
                {
                    exit = false;
                }
                else
                {
                    exit = true;
                }
            }





        }
    }
}
