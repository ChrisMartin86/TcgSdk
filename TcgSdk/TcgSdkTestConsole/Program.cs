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
        static void Main()
        {
            var param = new TcgSdkRequestParameter("name", "mirrodin", false, false) ;

            var cards = ITcgSdkResponseFactory<MagicSet>.Get(TcgSdkResponseType.MagicSet, new TcgSdkRequestParameter[] { param });
        }
    }
}
