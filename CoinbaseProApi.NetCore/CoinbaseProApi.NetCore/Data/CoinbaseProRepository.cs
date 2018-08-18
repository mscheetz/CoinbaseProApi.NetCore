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
        /// <param name="useSandbox">Use sandbox api (default = false)</param>
        public CoinbaseProRepository(bool useSandbox = false)
        {
            LoadRepository(useSandbox);
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiInfo">ApiInformation for authentication</param>
        /// <param name="useSandbox">Use sandbox api (default = false)</param>
        public CoinbaseProRepository(ApiInformation apiInfo, bool useSandbox = false)
        {
            _apiInfo = apiInfo;
            LoadRepository(useSandbox);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiKey">Api key</param>
        /// <param name="apiSecret">Api key secret</param>
        /// <param name="extraValue">Api key extra value</param>
        /// <param name="useSandbox">Use sandbox api (default = false)</param>
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
        /// <param name="configPath">Path to configuration file</param>
        /// <param name="useSandbox">Use sandbox api (default = false)</param>
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
            _helper = new Helper();
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

        #region Private endpoints

        /// <summary>
        /// Get account balances for user
        /// </summary>
        /// <returns>Accout object array</returns>
        public async Task<Account[]> GetAccounts()
        {
            var req = new Request
            {
                method = "GET",
                path = "/accounts",
                body = string.Empty
            };
            var url = baseUrl + req.path;

            var accountList = await _restRepo.GetApi<Account[]>(url, GetRequestHeaders(true, req));

            return accountList;
        }

        /// <summary>
        /// Get account balances for user
        /// </summary>
        /// <param name="id">String of account id</param>
        /// <returns>Accout object</returns>
        public async Task<Account> GetAccountBalance(string id)
        {
            var req = new Request
            {
                method = "GET",
                path = $"/accounts/{id}",
                body = string.Empty
            };
            var url = baseUrl + req.path;

            var accountList = await _restRepo.GetApi<Account>(url, GetRequestHeaders(true, req));

            return accountList;
        }

        /// <summary>
        /// Get account history for user
        /// </summary>
        /// <param name="id">String of account id</param>
        /// <returns>AccountHistory object array</returns>
        public async Task<AccountHistory[]> GetAccountHistory(string id)
        {
            var req = new Request
            {
                method = "GET",
                path = $"/accounts/{id}/ledger",
                body = string.Empty
            };
            var url = baseUrl + req.path;

            var accountHistory = await _restRepo.GetApi<AccountHistory[]>(url, GetRequestHeaders(true, req));

            return accountHistory;
        }

        /// <summary>
        /// Get account holds for user
        /// </summary>
        /// <param name="id">String of account id</param>
        /// <returns>AccoutHold object array</returns>
        public async Task<AccountHold[]> GetAccountHolds(string id)
        {
            var req = new Request
            {
                method = "GET",
                path = $"/accounts/{id}/holds",
                body = string.Empty
            };
            var url = baseUrl + req.path;

            var accountHistory = await _restRepo.GetApi<AccountHold[]>(url, GetRequestHeaders(true, req));

            return accountHistory;
        }

        /// <summary>
        /// Place a basic market order trade
        /// </summary>
        /// <param name="side">Buy or sell</param>
        /// <param name="pair">Trading pair</param>
        /// <returns>OrderResponse object</returns>
        public async Task<OrderResponse> PlaceMarketOrder(SIDE side, string pair)
        {
            var tradingPair = _helper.CreateDashedPair(pair);
            var tradeParams = new Dictionary<string, object>();
            tradeParams.Add("product_id", tradingPair);
            tradeParams.Add("side", side.ToString().ToLower());
            tradeParams.Add("type", TradeType.MARKET.ToString().ToLower());

            return await OnPlaceTrade(tradeParams);
        }

        /// <summary>
        /// Place a market order trade
        /// </summary>
        /// <param name="side">Buy or sell</param>
        /// <param name="pair">Trading pair</param>
        /// <param name="size">Amount in BTC</param>
        /// <param name="funds">Amount of quote currency to use</param>
        /// <returns>OrderResponse object</returns>
        public async Task<OrderResponse> PlaceMarketOrder(SIDE side, string pair, decimal size = 0.00000000M, decimal funds = 0.00000000M)
        {
            var tradingPair = _helper.CreateDashedPair(pair);
            var tradeParams = new Dictionary<string, object>();
            tradeParams.Add("product_id", tradingPair);
            tradeParams.Add("side", side.ToString().ToLower());
            tradeParams.Add("type", TradeType.MARKET.ToString().ToLower());
            if(size > 0.00000000M)
            {
                tradeParams.Add("size", size);
            }
            if (funds > 0.00000000M)
            {
                tradeParams.Add("funds", funds);
            }

            return await OnPlaceTrade(tradeParams);
        }

        /// <summary>
        /// Place a basic limit order trade
        /// </summary>
        /// <param name="side">Buy or sell</param>
        /// <param name="pair">Trading pair</param>
        /// <param name="price">Trading price</param>
        /// <param name="size">Amount in trade</param>
        /// <returns>OrderResponse object</returns>
        public async Task<OrderResponse> PlaceLimitOrder(SIDE side, string pair, decimal price, decimal size)
        {
            var tradingPair = _helper.CreateDashedPair(pair);
            var tradeParams = new Dictionary<string, object>();
            tradeParams.Add("product_id", tradingPair);
            tradeParams.Add("side", side.ToString().ToLower());
            tradeParams.Add("type", TradeType.MARKET.ToString().ToLower());
            tradeParams.Add("price", price);
            tradeParams.Add("size", size);
            tradeParams.Add("post_only", true);

            return await OnPlaceTrade(tradeParams);
        }

        /// <summary>
        /// Place a limit order trade
        /// </summary>
        /// <param name="side">Buy or sell</param>
        /// <param name="pair">Trading pair</param>
        /// <param name="price">Trading price</param>
        /// <param name="size">Amount in trade</param>
        /// <param name="tif">Time in force (default = GTC)</param>
        /// <param name="tca">Cancel after (default = none)</param>
        /// <returns>OrderResponse object</returns>
        public async Task<OrderResponse> PlaceLimitOrder(SIDE side, string pair, decimal price, decimal size, TimeInForce tif = TimeInForce.GTC, TradeCancelAfter tca = TradeCancelAfter.NONE)
        {
            var tradingPair = _helper.CreateDashedPair(pair);
            var tradeParams = new Dictionary<string, object>();
            tradeParams.Add("product_id", tradingPair);
            tradeParams.Add("side", side.ToString().ToLower());
            tradeParams.Add("type", TradeType.MARKET.ToString().ToLower());
            tradeParams.Add("price", price);
            tradeParams.Add("size", size);
            if (tif != TimeInForce.IOC && tif != TimeInForce.FOK)
            {
                tradeParams.Add("post_only", true);
            }
            tradeParams.Add("time_in_force", tif.ToString().ToLower());
            if(tca != TradeCancelAfter.NONE && tif == TimeInForce.GTT)
            {
                tradeParams.Add("cancel_after", tca.ToString().ToLower());
            }

            return await OnPlaceTrade(tradeParams);
        }

        /// <summary>
        /// Place an order trade
        /// </summary>
        /// <param name="tradeParams">TradeParams for setting the trade</param>
        /// <returns>OrderResponse object</returns>
        public async Task<OrderResponse> PlaceTrade(TradeParams tradeParams)
        {
            return await OnPlaceTrade(tradeParams);
        }

        /// <summary>
        /// Place a stop limit trade
        /// </summary>
        /// <param name="tradeParams">GDAXStopLostParams for setting the SL</param>
        /// <returns>GDAXOrderResponse object</returns>
        public async Task<OrderResponse> PlaceStopOrder(StopType type, SIDE side, string pair, decimal price, decimal stop_price, decimal size)
        {
            var tradingPair = _helper.CreateDashedPair(pair);
            var tradeParams = new Dictionary<string, object>();
            tradeParams.Add("product_id", tradingPair);
            tradeParams.Add("side", side.ToString().ToLower());
            tradeParams.Add("type", type.ToString().ToLower());
            tradeParams.Add("price", price);
            tradeParams.Add("stop_price", stop_price);
            tradeParams.Add("size", size);

            return await OnPlaceTrade(tradeParams);
        }

        /// <summary>
        /// Place a stop limit trade
        /// </summary>
        /// <param name="tradeParams">GDAXStopLostParams for setting the SL</param>
        /// <returns>GDAXOrderResponse object</returns>
        public async Task<OrderResponse> PlaceStopOrder(StopLossParams tradeParams)
        {
            return await OnPlaceTrade(tradeParams);
        }

        /// <summary>
        /// Place a limit order trade
        /// </summary>
        /// <param name="tradeParams">GDAXTradeParams for setting the trade</param>
        /// <returns>GDAXOrderResponse object</returns>
        private async Task<OrderResponse> OnPlaceTrade(Dictionary<string, object> tradeParams)
        {
            var req = new Request
            {
                method = "POST",
                path = "/orders",
                body = JsonConvert.SerializeObject(tradeParams)
            };
            var url = baseUrl + req.path;

            var response = await _restRepo.PostApi<OrderResponse, Dictionary<string, object>>(url, tradeParams, GetRequestHeaders(true, req));
            return response;
        }

        /// <summary>
        /// Place a limit order trade
        /// </summary>
        /// <param name="tradeParams">GDAXTradeParams for setting the trade</param>
        /// <returns>GDAXOrderResponse object</returns>
        private async Task<OrderResponse> OnPlaceTrade(TradeParams tradeParams)
        {
            var tradingPair = _helper.CreateDashedPair(tradeParams.product_id);
            tradeParams.product_id = tradingPair;
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
        /// Place a limit order trade
        /// </summary>
        /// <param name="tradeParams">GDAXTradeParams for setting the trade</param>
        /// <returns>GDAXOrderResponse object</returns>
        private async Task<OrderResponse> OnPlaceTrade(StopLossParams tradeParams)
        {
            var tradingPair = _helper.CreateDashedPair(tradeParams.product_id);
            tradeParams.product_id = tradingPair;
            tradeParams.post_only = true;
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
        /// Cancel an open order
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Boolean result of cancel request</returns>
        public async Task<bool> CancelOrder(string id)
        {
            var req = new Request
            {
                method = "DELETE",
                path = $"/orders/{id}",
                body = ""
            };
            var url = baseUrl + req.path;

            var response = await _restRepo.DeleteApi<string>(url, GetRequestHeaders(true, req));

            return response == null ? true : false;
        }

        /// <summary>
        /// Cancel all open orders
        /// </summary>
        /// <returns>String array of canceled order ids</returns>
        public async Task<string[]> CancelOrders()
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
        /// Get all open orders
        /// </summary>
        /// <returns>OrderResponse array</returns>
        public async Task<OrderResponse[]> GetOrders()
        {
            return await GetOrders("", OrderStatus.ALL);
        }

        /// <summary>
        /// Get open orders
        /// </summary>
        /// <param name="pair">Trading pair (default = "")</param>
        /// <param name="status">Order status (default = all)</param>
        /// <returns>OrderResponse array</returns>
        public async Task<OrderResponse[]> GetOrders(string pair = "", OrderStatus status = OrderStatus.ALL)
        {
            var tradingPair = string.Empty;
            var queryParam = string.Empty;
            if (pair != "")
            {
                tradingPair = _helper.CreateDashedPair(pair);
                queryParam = $"?product_id={tradingPair}";
            }
            if(status != OrderStatus.ALL)
            {
                queryParam += !string.IsNullOrEmpty(queryParam) ? "&" : "?";
                queryParam = $"status={status.ToString().ToLower()}";
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
        /// Get details of an order
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Order object</returns>
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
        /// <returns>Fill array</returns>
        public async Task<Fill[]> GetFills()
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
        /// Get user's 30-day trailing volume for all pairs
        /// </summary>
        /// <returns>TrailingVolume object array</returns>
        public async Task<TrailingVolume[]> GetTrailingVolume()
        {
            var req = new Request
            {
                method = "GET",
                path = $"/users/self/trailing-volume",
                body = ""
            };
            var url = baseUrl + req.path;

            var response = await _restRepo.GetApiStream<TrailingVolume[]>(url, GetRequestHeaders(true, req));

            return response;
        }

        #endregion Private endpoints

        #region Public endpoints

        /// <summary>
        /// Get available trading pairs
        /// </summary>
        /// <returns>ExchangeProduct object array</returns>
        public async Task<ExchangeProduct[]> GetTradingPairs()
        {
            var url = baseUrl + $"/products";

            var response = await _restRepo.GetApiStream<ExchangeProduct[]>(url, GetRequestHeaders());

            return response;
        }

        /// <summary>
        /// Get Current Order book
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="level">Request level, default = 2</param>
        /// <returns>ProductsOrderBookResponse object</returns>
        public async Task<OrderBookResponse> GetOrderBook(string pair, int level = 2)
        {
            var tradingPair = _helper.CreateDashedPair(pair);
            level = level < 1 
                    ? 1 
                    : level > 3 
                    ? 3 
                    : level;

            var url = baseUrl + $"/products/{tradingPair}/book?level={level}";

            var response = await _restRepo.GetApiStream<OrderBookResponse>(url, GetRequestHeaders());

            return response;
        }

        /// <summary>
        /// Get current ticker for a pair
        /// </summary>
        /// <returns>Ticker object</returns>
        public async Task<Ticker> GetTicker(string pair)
        {
            var tradingPair = _helper.CreateDashedPair(pair);

            var url = baseUrl + $"/products/{tradingPair}/ticker";

            var response = await _restRepo.GetApiStream<Ticker>(url, GetRequestHeaders());

            return response;
        }

        /// <summary>
        /// Get recent trades
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <returns>GdaxTrade array</returns>
        public async Task<Trade[]> GetTrades(string pair)
        {
            var tradingPair = _helper.CreateDashedPair(pair);
            var url = baseUrl + $"/products/{tradingPair}/trades";

            var response = await _restRepo.GetApiStream<Trade[]>(url, GetRequestHeaders());

            return response;
        }

        /// <summary>
        /// Get historic rates
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="granularity">Candle size</param>
        /// <param name="candleCount">Number of candles</param>
        /// <returns>HistoricRates array</returns>
        public async Task<HistoricRates[]> GetHistoricRates(string pair, Granularity granularity, int candleCount)
        {
            return await OnGetHistoricRates(pair, null, null, granularity, candleCount);
        }

        /// <summary>
        /// Get historic rates
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="endTime">End time of candles</param>
        /// <param name="granularity">Candle size</param>
        /// <param name="candleCount">Number of candles</param>
        /// <returns>HistoricRates array</returns>
        public async Task<HistoricRates[]> GetHistoricRates(string pair, DateTime endTime, Granularity granularity, int candleCount)
        {
            return await OnGetHistoricRates(pair, null, endTime, granularity, candleCount);
        }

        /// <summary>
        /// Get historic rates
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="startTime">Start time of candles</param>
        /// <param name="endTime">End time of candles</param>
        /// <param name="granularity">Candle size</param>
        /// <returns>HistoricRates array</returns>
        public async Task<HistoricRates[]> GetHistoricRates(string pair, DateTime startTime, DateTime endTime, Granularity granularity)
        {
            return await OnGetHistoricRates(pair, startTime, endTime, granularity, 0);
        }

        /// <summary>
        /// Get historic rates
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="startTime">Start time of candles</param>
        /// <param name="endTime">End time of candles</param>
        /// <param name="granularity">Candle size</param>
        /// <param name="candleCount">Number of candles</param>
        /// <returns>HistoricRates array</returns>
        private async Task<HistoricRates[]> OnGetHistoricRates(string pair, DateTime? startTime, DateTime? endTime, Granularity granularity, int candleCount)
        {
            DateTime endDT = endTime == null ? DateTime.UtcNow : (DateTime)endTime;
            DateTime startDT = DateTime.UtcNow;
            if(startTime == null)
            {
                startDT = _helper.GetFromUnixTime(endDT, granularity, candleCount);
            }
            else
            {
                startDT = (DateTime)startTime;
            }
            var startISO = _helper.GetISO8601Date(startDT);
            var endISO = _helper.GetISO8601Date(endDT);
            var tradingPair = _helper.CreateDashedPair(pair);
            var longGranularity = _helper.GranularityToNumber(granularity);
            var url = baseUrl + $"/products/{tradingPair}/candles?start={startISO}&end={endISO}&granularity={longGranularity}";

            var response = await _restRepo.GetApiStream<HistoricRates[]>(url, GetRequestHeaders());

            return response;
        }

        /// <summary>
        /// Get 24hr stats for a trading pair
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <returns>PairStats object</returns>
        public async Task<PairStats> GetStats(string pair)
        {
            var tradingPair = _helper.CreateDashedPair(pair);
            var url = baseUrl + $"/products/{tradingPair}/stats";

            var response = await _restRepo.GetApiStream<PairStats>(url, GetRequestHeaders());

            return response;
        }

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns>Currency array</returns>
        public async Task<Currency[]> GetCurrencies()
        {
            var url = baseUrl + $"/currencies";

            var response = await _restRepo.GetApiStream<Currency[]>(url, GetRequestHeaders());

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

        #endregion Public endpoints

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
