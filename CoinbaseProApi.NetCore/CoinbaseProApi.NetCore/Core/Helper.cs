using CoinbaseProApi.NetCore.Entities;
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

        /// <summary>
        /// Get number vaule of granularity
        /// </summary>
        /// <param name="granularity">Granularity enum value</param>
        /// <returns>Converted long value</returns>
        public long GranularityToNumber(Granularity granularity)
        {
            switch(granularity)
            {
                case Granularity.OneM:
                    return 60;
                case Granularity.FiveM:
                    return 300;
                case Granularity.FifteenM:
                    return 900;
                case Granularity.OneH:
                    return 3600;
                case Granularity.SixH:
                    return 21600;
                case Granularity.OneD:
                    return 86400;
                default:
                    return 60;

            }
        }
    }
}
