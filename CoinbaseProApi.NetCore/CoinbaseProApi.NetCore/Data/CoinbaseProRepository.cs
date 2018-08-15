using CoinbaseProApi.NetCore.Core;
using CoinbaseProApi.NetCore.Data.Interfaces;
using CoinbaseProApi.NetCore.Entities;
using DateTimeHelpers;
using FileRepository;
using Newtonsoft.Json;
using RESTApiAccess;
using RESTApiAccess.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinbaseProApi.NetCore.Data
{
    public class CoinbaseProRepository : ICoinbaseProRepository
    {
        private IRESTRepository _restRepo;
        private DateTimeHelper _dtHelper;
        private Helper _helper;
        private Security _security;
        private ApiInformation _apiInfo;
        private string baseUrl = "https://api.pro.coinbase.com";
        private IFileRepository _fileRepo;

        /// <summary>
        /// Constructor
        /// </summary>
        public CoinbaseProRepository(bool useSandbox = false)
        {
            LoadRepository(useSandbox);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url">Base url value</param>
        public CoinbaseProRepository(string apiKey, string apiSecret, string extraValue, bool useSandbox = false)
        {
            _apiInfo = new ApiInformation
            {
                apiKey = apiKey,
                apiSecret = apiSecret,
                extraValue = extraValue
            };
            LoadRepository(useSandbox);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiInformation">Api Information</param>
        public CoinbaseProRepository(string configPath, bool useSandbox = false)
        {
            IFileRepository _fileRepo = new FileRepository.FileRepository();

            if (_fileRepo.FileExists(configPath))
            {
                _apiInfo = _fileRepo.GetDataFromFile<ApiInformation>(configPath);
                LoadRepository(useSandbox);
            }
            else
            {
                throw new Exception("Config file not found");
            }
        }

        /// <summary>
        /// Load repository
        /// </summary>
        /// <param name="key">Api key value (default = "")</param>
        /// <param name="secret">Api secret value (default = "")</param>
        private void LoadRepository(bool useSandbox)
        {
            _security = new Security();
            _restRepo = new RESTRepository();
            baseUrl = useSandbox 
                ? "https://public.sandbox.pro.coinbase.com"
                : "https://api.pro.coinbase.com";
            _dtHelper = new DateTimeHelper();
        }

        /// <summary>
        /// Check if the Exchange Repository is ready for trading
        /// </summary>
        /// <returns>Boolean of validation</returns>
        public bool ValidateExchangeConfigured()
        {
            var ready = string.IsNullOrEmpty(_apiInfo.apiKey) ? false : true;
            if (!ready)
                return false;

            return string.IsNullOrEmpty(_apiInfo.apiSecret) ? false : true;
        }

        /// <summary>
        /// Get Current Order book
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="level">Request level, default = 2</param>
        /// <returns>ProductsOrderBookResponse object</returns>
        public async Task<OrderBookResponse> GetOrderBook(string pair, int level = 2)
        {
            var gdaxPair = _helper.CreateDashedPair(pair);
            var url = baseUrl + $"/products/{gdaxPair}/book?level={level}";

            var response = await _restRepo.GetApiStream<OrderBookResponse>(url, GetRequestHeaders());

            return response;
        }

        /// <summary>
        /// Get recent trades
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <returns>GdaxTrade array</returns>
        public async Task<Trade[]> GetTrades(string pair)
        {
            var gdaxPair = _helper.CreateDashedPair(pair);
            var url = baseUrl + $"/products/{gdaxPair}/trades";

            var response = await _restRepo.GetApiStream<Trade[]>(url, GetRequestHeaders());

            return response;
        }
        
        /// <summary>
        /// Convert GdaxTrade array to BotStick array
        /// </summary>
        /// <param name="pair">String of trading pair</param>
        /// <param name="range">Size of array to return</param>
        /// <returns>BotStick array</returns>
        public Candlestick[] GetCandleSticks(string pair, int range)
        {
            var trades = GetTrades(pair).Result;
            var close = trades[0].Price;
            var maxDate = trades[0].Time;
            maxDate = maxDate.AddSeconds(-maxDate.Second);
            var tradesPrev = trades.Where(t => t.Time < maxDate).OrderByDescending(t => t.Time).ToArray();
            var prevClose = 0.0M;
            if (tradesPrev.Length > 0)
            {
                prevClose = tradesPrev[0].Price;
            }
            var grouped = trades.GroupBy(
                t => new
                {
                    Date = _dtHelper.LocalToUnixTime(t.Time.AddSeconds(-t.Time.Second).AddMilliseconds(-t.Time.Millisecond))
                })
                .Select(
                t => new Candlestick
                {
                    closeTime = t.Max(m => _dtHelper.LocalToUnixTime(m.Time.AddSeconds(-m.Time.Second).AddMilliseconds(-m.Time.Millisecond))),
                    high = t.Max(m => m.Price),
                    low = t.Min(m => m.Price),
                    volume = t.Sum(m => m.Size)
                }).ToList();

            grouped[0].close = close;
            if (grouped.Count > 1)
            {
                grouped[1].close = prevClose;
            }

            int size = grouped.Count() < range ? grouped.Count() : range;

            var groupedArray = grouped.Take(size).ToArray();

            Array.Reverse(groupedArray);

            return groupedArray;
        }

        /// <summary>
        /// Get Balances for GDAX account
        /// </summary>
        /// <returns>Accout object</returns>
        public async Task<Account[]> GetBalance()
        {
            var url = baseUrl + "/accounts";
            var req = new Request
            {
                method = "GET",
                path = "/accounts",
                body = string.Empty
            };

            var accountList = await _restRepo.GetApi<Account[]>(url, GetRequestHeaders(true, req));

            return accountList;
        }

        /// <summary>
        /// Place a limit order trade
        /// </summary>
        /// <param name="tradeParams">GDAXTradeParams for setting the trade</param>
        /// <returns>GDAXOrderResponse object</returns>
        public async Task<OrderResponse> PlaceTrade(TradeParams tradeParams)
        {
            var gdaxPair = _helper.CreateDashedPair(tradeParams.product_id);
            tradeParams.product_id = gdaxPair;
            tradeParams.post_only = true;
            var req = new Request
            {
                method = "POST",
                path = "/orders",
                body = JsonConvert.SerializeObject(tradeParams)
            };
            var url = baseUrl + req.path;

            var response = await _restRepo.PostApi<OrderResponse, TradeParams>(url, tradeParams, GetRequestHeaders(true, req));
            return response;
        }

        /// <summary>
        /// Place a stop limit trade
        /// </summary>
        /// <param name="tradeParams">GDAXStopLostParams for setting the SL</param>
        /// <returns>GDAXOrderResponse object</returns>
        public async Task<OrderResponse> PlaceStopLimit(StopLossParams tradeParams)
        {
            var gdaxPair = _helper.CreateDashedPair(tradeParams.product_id);
            tradeParams.product_id = gdaxPair;
            var req = new Request
            {
                method = "POST",
                path = "/orders",
                body = JsonConvert.SerializeObject(tradeParams)
            };
            var url = baseUrl + req.path;

            var response = await _restRepo.PostApi<OrderResponse, StopLossParams>(url, tradeParams, GetRequestHeaders(true, req));

            return response;
        }

        /// <summary>
        /// Get details of an order
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>OrderResponse object</returns>
        public async Task<Order> GetOrder(string id)
        {
            var req = new Request
            {
                method = "GET",
                path = $"/orders/{id}",
                body = ""
            };
            var url = baseUrl + req.path;

            var response = await _restRepo.GetApi<Order>(url, GetRequestHeaders(true, req));

            return response;
        }

        /// <summary>
        /// Get all fills
        /// </summary>
        /// <returns>GDAXFill array</returns>
        public async Task<Fill[]> GetOrders()
        {
            var req = new Request
            {
                method = "GET",
                path = $"/fills",
                body = ""
            };
            var url = baseUrl + req.path;

            var response = await _restRepo.GetApiStream<Fill[]>(url, GetRequestHeaders(true, req));

            return response;
        }

        /// <summary>
        /// Get all open orders
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <returns>GDAXOrderResponse array</returns>
        public async Task<OrderResponse[]> GetOpenOrders(string pair = "")
        {
            var gdaxPair = string.Empty;
            var queryParam = string.Empty;
            if (pair != "")
            {
                gdaxPair = _helper.CreateDashedPair(pair);
                queryParam = $"?product_id={gdaxPair}";
            }
            var req = new Request
            {
                method = "GET",
                path = $"/orders{queryParam}",
                body = ""
            };
            var url = baseUrl + req.path + queryParam;

            var response = await _restRepo.GetApiStream<OrderResponse[]>(url, GetRequestHeaders(true, req));

            return response;
        }

        /// <summary>
        /// Cancel all open trades
        /// </summary>
        /// <returns>CancelOrderResponse object</returns>
        public async Task<string[]> CancelAllTradesRest()
        {
            var req = new Request
            {
                method = "DELETE",
                path = "/orders",
                body = ""
            };
            var url = baseUrl + req.path;

            var response = await _restRepo.DeleteApi<string[]>(url, GetRequestHeaders(true, req));
            return response;
        }

        /// <summary>
        /// Add request headers to api call
        /// </summary>
        /// <returns>Dictionary of request headers</returns>
        private Dictionary<string, string> GetRequestHeaders(bool secure = false, Request request = null)
        {
            string utcDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var headers = new Dictionary<string, string>();

            headers.Add("User-Agent", "GDAX Request");

            if (secure)
            {
                if (request != null)
                {
                    string nonce = _dtHelper.UTCtoUnixTime().ToString(System.Globalization.CultureInfo.InvariantCulture);
                    var body = request.body == "" ? string.Empty : request.body;
                    string message = $"{nonce}{request.method}{request.path}{body}";
                    headers.Add("CB-ACCESS-KEY", _apiInfo.apiKey);
                    headers.Add("CB-ACCESS-TIMESTAMP", nonce);
                    headers.Add("CB-ACCESS-SIGN", CreateSignature(message));
                    headers.Add("CB-ACCESS-PASSPHRASE", _apiInfo.extraValue);
                }
                headers.Add("CB-VERSION", utcDate);
            }
            return headers;
        }

        /// <summary>
        /// Create signature for message
        /// </summary>
        /// <param name="message">Message to sign</param>
        /// <returns>String of signature</returns>
        private string CreateSignature(string message)
        {
            var hmac = _security.GetHMACSignature(_apiInfo.apiSecret, message);
            return hmac;
        }
    }
}
