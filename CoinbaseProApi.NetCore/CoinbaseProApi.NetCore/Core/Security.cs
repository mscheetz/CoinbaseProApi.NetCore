using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CoinbaseProApi.NetCore.Core
{
    public class Security
    {
        /// <summary>
        /// Get HMAC Signature
        /// </summary>
        /// <param name="message">Message to sign</param>
        /// <param name="keySecret">Api key secret</param>
        /// <returns>string of signed message</returns>
        public string GetHMACSignature(string totalParams, string secretKey)
        {
            byte[] keyByte = Convert.FromBase64String(secretKey);
            byte[] messageByte = Encoding.UTF8.GetBytes(totalParams);
            using (var hmac = new HMACSHA256(keyByte))
            {
                byte[] hashMessage = hmac.ComputeHash(messageByte);
                return Convert.ToBase64String(hashMessage);
            }
        }
    }
}
