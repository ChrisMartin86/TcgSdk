using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcgSdk.Common
{
    public static class Utility
    {
        private static Random random = new Random();

        public static int GetRandomInt(int inclusiveStartBoundary, int exclusiveEndBoundary)
        {
            return random.Next(inclusiveStartBoundary, exclusiveEndBoundary);
        }

        public static IEnumerable<int> RollDice(int numberOfSides, int numberOfDice)
        {
            List<int> resultArray = new List<int>();

            for (int i = 0; i < numberOfDice; i++)
            {
                resultArray.Add(GetRandomInt(1, (numberOfSides + 1)));
            }

            return resultArray;
        }
    }
}
