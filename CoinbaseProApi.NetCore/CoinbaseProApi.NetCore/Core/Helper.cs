using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseProApi.NetCore.Core
{
    public class Helper
    {
        /// <summary>
        /// Creates dashed pair (ie BTC-ETH)
        /// </summary>
        /// <param name="pair">String of pair</param>
        /// <returns>String of pair</returns>
        public string CreateDashedPair(string pair)
        {
            if (pair.IndexOf("-") < 0)
            {
                pair = pair.Substring(0, 3) + "-" + pair.Substring(3);
            }

            return pair;
        }
    }
}
