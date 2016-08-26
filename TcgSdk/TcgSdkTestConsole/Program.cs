using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcgSdk;
using TcgSdk.Magic;
using TcgSdk.Common;

namespace TcgSdkTestConsole
{
    class Program
    {
        static void Main()
        {
            var filter = new Dictionary<string, string>();

            filter.Add("name", "plains");

            var cards = MagicCard.Get(filter);
        }
    }
}
