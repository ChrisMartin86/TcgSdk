using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcgSdk;
using TcgSdk.Pokemon;

namespace TcgSdkTestConsole
{
    class Program
    {
        static void Main()
        {
            string filter = "?name=Charizard";

            var cards = PokemonCard.Get(null);
        }
    }
}
