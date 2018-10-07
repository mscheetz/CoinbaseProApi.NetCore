using CoinbaseProApi.NetCore.Data;
using CoinbaseProApi.NetCore.Data.Interfaces;
using CoinbaseProApi.NetCore.Entities;
using FileRepository;
using System;
using System.Threading.Tasks;

namespace CoinbaseProApi.NetCore
{
    public class CoinbaseProClient
    {
        private ICoinbaseProRepository _repository;

        /// <summary>
        /// Constructor - no authentication
        /// </summary>
        /// <param name="useSandbox">Use sandbox api (default = false)</param>
        public CoinbaseProClient(bool useSandbox = false)
        {
            _repository = new CoinbaseProRepository(useSandbox);
        }

        /// <summary>
        /// Constructor - with authentication
        /// </summary>
        /// <param name="apiKey">Api key</param>
        /// <param name="apiSecret">Api secret</param>
        /// <param name="apiPassword">Api password</param>
        /// <param name="useSandbox">Use sandbox api (default = false)</param>
        public CoinbaseProClient(string apiKey, string apiSecret, string apiPassword, bool useSandbox = false)
        {
            _repository = new CoinbaseProRepository(apiKey, apiSecret, apiPassword, useSandbox);
        }

        /// <summary>
        /// Constructor - with authentication
        /// </summary>
        /// <param name="configPath">Path to config file</param>
        /// <param name="useSandbox">Use sandbox api (default = false)</param>
        public CoinbaseProClient(string configPath, bool useSandbox = false)
        {
            IFileRepository _fileRepo = new FileRepository.FileRepository();

            if (_fileRepo.FileExists(configPath))
            {
                var apiInfo = _fileRepo.GetDataFromFile<ApiInformation>(configPath);

                _repository = new CoinbaseProRepository(apiInfo, useSandbox);
            }
            else
            {
                throw new Exception("Config file not found!");
            }
        }
        /// <summary>
        /// Check if the Exchange Repository is ready for trading
        /// </summary>
        /// <returns>Boolean of validation</returns>
        public bool ValidateExchangeConfigured()
        {
            return _repository.ValidateExchangeConfigured();
        }

        /// <summary>
        /// Get account balances for user
        /// </summary>
        /// <returns>Accout object array</returns>
        public Account[] GetAccounts()
        {
            return _repository.GetAccounts().Result;
        }

        /// <summary>
        /// Get account balance for user
        /// </summary>
        /// <param name="id">Account id</param>
        /// <returns>Accout object array</returns>
        public Account GetAccount(string id)
        {
            return _repository.GetAccount(id).Result;
        }

        /// <summary>
        /// Get account balances for user
        /// </summary>
        /// <param name="id">String of account id</param>
        /// <returns>Accout object</returns>
        public Account GetAccountBalance(string id)
        {
            return _repository.GetAccountBalance(id).Result;
        }

        /// <summary>
        /// Get account history for user
        /// </summary>
        /// <param name="id">String of account id</param>
        /// <returns>AccountHistory object array</returns>
        public AccountHistory[] GetAccountHistory(string id)
        {
            return _repository.GetAccountHistory(id).Result;
        }

        /// <summary>
        /// Get account holds for user
        /// </summary>
        /// <param name="id">String of account id</param>
        /// <returns>AccoutHold object array</returns>
        public AccountHold[] GetAccountHolds(string id)
        {
            return _repository.GetAccountHolds(id).Result;
        }

        /// <summary>
        /// Place a basic market order trade
        /// </summary>
        /// <param name="side">Buy or sell</param>
        /// <param name="pair">Trading pair</param>
        /// <param name="size">Size of trade</param>
        /// <returns>OrderResponse object</returns>
        public OrderResponse PlaceMarketOrder(SIDE side, string pair, decimal size)
        {
            return _repository.PlaceMarketOrder(side, pair, size).Result;
        }

        /// <summary>
        /// Place a market order trade
        /// </summary>
        /// <param name="side">Buy or sell</param>
        /// <param name="pair">Trading pair</param>
        /// <param name="size">Amount in BTC</param>
        /// <param name="funds">Amount of quote currency to use</param>
        /// <returns>OrderResponse object</returns>
        public OrderResponse PlaceMarketOrder(SIDE side, string pair, decimal size = 0.00000000M, decimal funds = 0.00000000M)
        {
            return _repository.PlaceMarketOrder(side, pair, size, funds).Result;
        }

        /// <summary>
        /// Place a basic limit order trade
        /// </summary>
        /// <param name="side">Buy or sell</param>
        /// <param name="pair">Trading pair</param>
        /// <param name="price">Trading price</param>
        /// <param name="size">Amount in trade</param>
        /// <returns>OrderResponse object</returns>
        public OrderResponse PlaceLimitOrder(SIDE side, string pair, decimal price, decimal size)
        {
            return _repository.PlaceLimitOrder(side, pair, price, size).Result;
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
        public OrderResponse PlaceLimitOrder(SIDE side, string pair, decimal price, decimal size, TimeInForce tif = TimeInForce.GTC, TradeCancelAfter tca = TradeCancelAfter.NONE)
        {
            return _repository.PlaceLimitOrder(side, pair, price, size, tif, tca).Result;
        }

        /// <summary>
        /// Place an order trade
        /// </summary>
        /// <param name="tradeParams">TradeParams for setting the trade</param>
        /// <returns>OrderResponse object</returns>
        public OrderResponse PlaceTrade(TradeParams tradeParams)
        {
            return _repository.PlaceTrade(tradeParams).Result;
        }

        /// <summary>
        /// Place a stop limit trade
        /// </summary>
        /// <param name="type">Stop Type</param>
        /// <param name="side">Trade side</param>
        /// <param name="pair">Trading pair</param>
        /// <param name="price">Price of trade</param>
        /// <param name="stop_price">Stop price</param>
        /// <param name="size">Size of trade</param>
        /// <returns>OrderResponse object</returns>
        public OrderResponse PlaceStopOrder(StopType type, SIDE side, string pair, decimal price, decimal stop_price, decimal size)
        {
            return _repository.PlaceStopOrder(type, side, pair, price, stop_price, size).Result;
        }

        /// <summary>
        /// Place a stop limit trade
        /// </summary>
        /// <param name="tradeParams">StopLostParams for setting the SL</param>
        /// <returns>OrderResponse object</returns>
        public OrderResponse PlaceStopOrder(StopLossParams tradeParams)
        {
            return _repository.PlaceStopOrder(tradeParams).Result;
        }

        /// <summary>
        /// Cancel an open order
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Boolean result of cancel request</returns>
        public bool CancelOrder(string id)
        {
            return _repository.CancelOrder(id).Result;
        }

        /// <summary>
        /// Cancel all open orders
        /// </summary>
        /// <returns>String array of canceled order ids</returns>
        public string[] CancelOrders()
        {
            return _repository.CancelOrders().Result;
        }

        /// <summary>
        /// Get all open orders
        /// </summary>
        /// <returns>OrderResponse array</returns>
        public OrderResponse[] GetOrders()
        {
            return _repository.GetOrders().Result;
        }

        /// <summary>
        /// Get open orders
        /// </summary>
        /// <param name="pair">Trading pair (default = "")</param>
        /// <param name="status">Order status (default = all)</param>
        /// <returns>OrderResponse array</returns>
        public OrderResponse[] GetOrders(string pair = "", OrderStatus status = OrderStatus.ALL)
        {
            return _repository.GetOrders(pair, status).Result;
        }

        /// <summary>
        /// Get details of an order
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Order object</returns>
        public Order GetOrder(string id)
        {
            return _repository.GetOrder(id).Result;
        }

        /// <summary>
        /// Get all fills
        /// </summary>
        /// <returns>Fill array</returns>
        public Fill[] GetFills()
        {
            return _repository.GetFills().Result;
        }

        /// <summary>
        /// Get user's 30-day trailing volume for all pairs
        /// </summary>
        /// <returns>TrailingVolume object array</returns>
        public TrailingVolume[] GetTrailingVolume()
        {
            return _repository.GetTrailingVolume().Result;
        }

        /// <summary>
        /// Get available trading pairs
        /// </summary>
        /// <returns>ExchangeProduct object array</returns>
        public ExchangeProduct[] GetTradingPairs()
        {
            return _repository.GetTradingPairs().Result;
        }

        /// <summary>
        /// Get Current Order book
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="level">Request level, default = 2</param>
        /// <returns>ProductsOrderBookResponse object</returns>
        public OrderBookResponse GetOrderBook(string pair, int level = 2)
        {
            return _repository.GetOrderBook(pair, level).Result;
        }

        /// <summary>
        /// Get current ticker for a pair
        /// </summary>
        /// <returns>Ticker object</returns>
        public Ticker GetTicker(string pair)
        {
            return _repository.GetTicker(pair).Result;
        }

        /// <summary>
        /// Get recent trades
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <returns>Trade array</returns>
        public Trade[] GetTrades(string pair)
        {
            return _repository.GetTrades(pair).Result;
        }

        /// <summary>
        /// Get historic rates
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="granularity">Candle size</param>
        /// <param name="candleCount">Number of candles</param>
        /// <returns>HistoricRates array</returns>
        public HistoricRates[] GetHistoricRates(string pair, Granularity granularity, int candleCount)
        {
            return _repository.GetHistoricRates(pair, granularity, candleCount).Result;
        }

        /// <summary>
        /// Get historic rates
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="endTime">End time of candles (UTC Time)</param>
        /// <param name="granularity">Candle size</param>
        /// <param name="candleCount">Number of candles</param>
        /// <returns>HistoricRates array</returns>
        public HistoricRates[] GetHistoricRates(string pair, DateTime endTime, Granularity granularity, int candleCount)
        {
            return _repository.GetHistoricRates(pair, endTime, granularity, candleCount).Result;
        }

        /// <summary>
        /// Get historic rates
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="startTime">Start time of candles (UTC Time)</param>
        /// <param name="endTime">End time of candles (UTC Time)</param>
        /// <param name="granularity">Candle size</param>
        /// <returns>HistoricRates array</returns>
        public HistoricRates[] GetHistoricRates(string pair, DateTime startTime, DateTime endTime, Granularity granularity)
        {
            return _repository.GetHistoricRates(pair, startTime, endTime, granularity).Result;
        }

        /// <summary>
        /// Get 24hr stats for a trading pair
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <returns>PairStats object</returns>
        public PairStats GetStats(string pair)
        {
            return _repository.GetStats(pair).Result;
        }

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns>Currency array</returns>
        public Currency[] GetCurrencies()
        {
            return _repository.GetCurrencies().Result;
        }

        /// <summary>
        /// Get account balances for user asyncronously
        /// </summary>
        /// <returns>Accout object array</returns>
        public async Task<Account[]> GetAccountsAsync()
        {
            return await _repository.GetAccounts();
        }

        /// <summary>
        /// Get account balance for user asyncronously
        /// </summary>
        /// <param name="id">Account id</param>
        /// <returns>Accout object array</returns>
        public async Task<Account> GetAccountAsync(string id)
        {
            return await _repository.GetAccount(id);
        }

        /// <summary>
        /// Get account balances for user asyncronously
        /// </summary>
        /// <param name="id">String of account id</param>
        /// <returns>Accout object</returns>
        public async Task<Account> GetAccountBalanceAsync(string id)
        {
            return await _repository.GetAccountBalance(id);
        }

        /// <summary>
        /// Get account history for user asyncronously
        /// </summary>
        /// <param name="id">String of account id</param>
        /// <returns>AccountHistory object array</returns>
        public async Task<AccountHistory[]> GetAccountHistoryAsync(string id)
        {
            return await _repository.GetAccountHistory(id);
        }

        /// <summary>
        /// Get account holds for user asyncronously
        /// </summary>
        /// <param name="id">String of account id</param>
        /// <returns>AccoutHold object array</returns>
        public async Task<AccountHold[]> GetAccountHoldsAsync(string id)
        {
            return await _repository.GetAccountHolds(id);
        }

        /// <summary>
        /// Place a basic market order trade asyncronously
        /// </summary>
        /// <param name="side">Buy or sell</param>
        /// <param name="pair">Trading pair</param>
        /// <param name="size">Size of trade</param>
        /// <returns>OrderResponse object</returns>
        public async Task<OrderResponse> PlaceMarketOrderAsync(SIDE side, string pair, decimal size)
        {
            return await _repository.PlaceMarketOrder(side, pair, size);
        }

        /// <summary>
        /// Place a market order trade asyncronously
        /// </summary>
        /// <param name="side">Buy or sell</param>
        /// <param name="pair">Trading pair</param>
        /// <param name="size">Amount in BTC</param>
        /// <param name="funds">Amount of quote currency to use</param>
        /// <returns>OrderResponse object</returns>
        public async Task<OrderResponse> PlaceMarketOrderAsync(SIDE side, string pair, decimal size = 0.00000000M, decimal funds = 0.00000000M)
        {
            return await _repository.PlaceMarketOrder(side, pair, size, funds);
        }

        /// <summary>
        /// Place a basic limit order trade asyncronously
        /// </summary>
        /// <param name="side">Buy or sell</param>
        /// <param name="pair">Trading pair</param>
        /// <param name="price">Trading price</param>
        /// <param name="size">Amount in trade</param>
        /// <returns>OrderResponse object</returns>
        public async Task<OrderResponse> PlaceLimitOrderAsync(SIDE side, string pair, decimal price, decimal size)
        {
            return await _repository.PlaceLimitOrder(side, pair, price, size);
        }

        /// <summary>
        /// Place a limit order trade asyncronously
        /// </summary>
        /// <param name="side">Buy or sell</param>
        /// <param name="pair">Trading pair</param>
        /// <param name="price">Trading price</param>
        /// <param name="size">Amount in trade</param>
        /// <param name="tif">Time in force (default = GTC)</param>
        /// <param name="tca">Cancel after (default = none)</param>
        /// <returns>OrderResponse object</returns>
        public async Task<OrderResponse> PlaceLimitOrderAsync(SIDE side, string pair, decimal price, decimal size, TimeInForce tif = TimeInForce.GTC, TradeCancelAfter tca = TradeCancelAfter.NONE)
        {
            return await _repository.PlaceLimitOrder(side, pair, price, size, tif, tca);
        }

        /// <summary>
        /// Place an order trade asyncronously
        /// </summary>
        /// <param name="tradeParams">TradeParams for setting the trade</param>
        /// <returns>OrderResponse object</returns>
        public async Task<OrderResponse> PlaceTradeAsync(TradeParams tradeParams)
        {
            return await _repository.PlaceTrade(tradeParams);
        }

        /// <summary>
        /// Place a stop limit trade asyncronously
        /// </summary>
        /// <param name="type">Stop Type</param>
        /// <param name="side">Trade side</param>
        /// <param name="pair">Trading pair</param>
        /// <param name="price">Price of trade</param>
        /// <param name="stop_price">Stop price</param>
        /// <param name="size">Size of trade</param>
        /// <returns>OrderResponse object</returns>
        public async Task<OrderResponse> PlaceStopOrderAsync(StopType type, SIDE side, string pair, decimal price, decimal stop_price, decimal size)
        {
            return await _repository.PlaceStopOrder(type, side, pair, price, stop_price, size);
        }

        /// <summary>
        /// Place a stop limit trade asyncronously
        /// </summary>
        /// <param name="tradeParams">StopLostParams for setting the SL</param>
        /// <returns>OrderResponse object</returns>
        public async Task<OrderResponse> PlaceStopOrderAsync(StopLossParams tradeParams)
        {
            return await _repository.PlaceStopOrder(tradeParams);
        }

        /// <summary>
        /// Cancel an open order asyncronously
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Boolean result of cancel request</returns>
        public async Task<bool> CancelOrderAsync(string id)
        {
            return await _repository.CancelOrder(id);
        }

        /// <summary>
        /// Cancel all open orders asyncronously
        /// </summary>
        /// <returns>String array of canceled order ids</returns>
        public async Task<string[]> CancelOrdersAsync()
        {
            return await _repository.CancelOrders();
        }

        /// <summary>
        /// Get all open orders asyncronously
        /// </summary>
        /// <returns>OrderResponse array</returns>
        public async Task<OrderResponse[]> GetOrdersAsync()
        {
            return await _repository.GetOrders();
        }

        /// <summary>
        /// Get open orders asyncronously
        /// </summary>
        /// <param name="pair">Trading pair (default = "")</param>
        /// <param name="status">Order status (default = all)</param>
        /// <returns>OrderResponse array</returns>
        public async Task<OrderResponse[]> GetOrdersAsync(string pair = "", OrderStatus status = OrderStatus.ALL)
        {
            return await _repository.GetOrders(pair, status);
        }

        /// <summary>
        /// Get details of an order asyncronously
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Order object</returns>
        public async Task<Order> GetOrderAsync(string id)
        {
            return await _repository.GetOrder(id);
        }

        /// <summary>
        /// Get all fills asyncronously
        /// </summary>
        /// <returns>Fill array</returns>
        public async Task<Fill[]> GetFillsAsync()
        {
            return await _repository.GetFills();
        }

        /// <summary>
        /// Get user's 30-day trailing volume for all pairs asyncronously
        /// </summary>
        /// <returns>TrailingVolume object array</returns>
        public async Task<TrailingVolume[]> GetTrailingVolumeAsync()
        {
            return await _repository.GetTrailingVolume();
        }

        /// <summary>
        /// Get available trading pairs asyncronously
        /// </summary>
        /// <returns>ExchangeProduct object array</returns>
        public async Task<ExchangeProduct[]> GetTradingPairsAsync()
        {
            return await _repository.GetTradingPairs();
        }

        /// <summary>
        /// Get Current Order book asyncronously
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="level">Request level, default = 2</param>
        /// <returns>ProductsOrderBookResponse object</returns>
        public async Task<OrderBookResponse> GetOrderBookAsync(string pair, int level = 2)
        {
            return await _repository.GetOrderBook(pair, level);
        }

        /// <summary>
        /// Get current ticker for a pair asyncronously
        /// </summary>
        /// <returns>Ticker object</returns>
        public async Task<Ticker> GetTickerAsync(string pair)
        {
            return await _repository.GetTicker(pair);
        }

        /// <summary>
        /// Get recent trades asyncronously
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <returns>Trade array</returns>
        public async Task<Trade[]> GetTradesAsync(string pair)
        {
            return await _repository.GetTrades(pair);
        }

        /// <summary>
        /// Get historic rates asyncronously
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="granularity">Candle size</param>
        /// <param name="candleCount">Number of candles</param>
        /// <returns>HistoricRates array</returns>
        public async Task<HistoricRates[]> GetHistoricRatesAsync(string pair, Granularity granularity, int candleCount)
        {
            return await _repository.GetHistoricRates(pair, granularity, candleCount);
        }

        /// <summary>
        /// Get historic rates asyncronously
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="endTime">End time of candles (UTC Time)</param>
        /// <param name="granularity">Candle size</param>
        /// <param name="candleCount">Number of candles</param>
        /// <returns>HistoricRates array</returns>
        public async Task<HistoricRates[]> GetHistoricRatesAsync(string pair, DateTime endTime, Granularity granularity, int candleCount)
        {
            return await _repository.GetHistoricRates(pair, endTime, granularity, candleCount);
        }

        /// <summary>
        /// Get historic rates asyncronously
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <param name="startTime">Start time of candles (UTC Time)</param>
        /// <param name="endTime">End time of candles (UTC Time)</param>
        /// <param name="granularity">Candle size</param>
        /// <returns>HistoricRates array</returns>
        public async Task<HistoricRates[]> GetHistoricRatesAsync(string pair, DateTime startTime, DateTime endTime, Granularity granularity)
        {
            return await _repository.GetHistoricRates(pair, startTime, endTime, granularity);
        }

        /// <summary>
        /// Get 24hr stats for a trading pair asyncronously
        /// </summary>
        /// <param name="pair">Trading pair</param>
        /// <returns>PairStats object</returns>
        public async Task<PairStats> GetStatsAsync(string pair)
        {
            return await _repository.GetStats(pair);
        }

        /// <summary>
        /// Get all currencies asyncronously
        /// </summary>
        /// <returns>Currency array</returns>
        public async Task<Currency[]> GetCurrenciesAsync()
        {
            return await _repository.GetCurrencies();
        }
    }
}
