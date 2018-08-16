using CoinbaseProApi.NetCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoinbaseProApi.NetCore.Data.Interfaces
{
    public interface ICoinbaseProRepository
    {
        /// <summary>
        /// Get account balances for user
        /// </summary>
        /// <returns>Accout object array</returns>
        Task<Account[]> GetAccounts();

        /// <summary>
        /// Get account balances for user
        /// </summary>
        /// <param name="id">String of account id</param>
        /// <returns>Accout object</returns>
        Task<Account> GetAccountBalance(string id);

        /// <summary>
        /// Get account history for user
        /// </summary>
        /// <param name="id">String of account id</param>
        /// <returns>AccountHistory object array</returns>
        Task<AccountHistory[]> GetAccountHistory(string id);

        /// <summary>
        /// Get account holds for user
        /// </summary>
        /// <param name="id">String of account id</param>
        /// <returns>AccoutHold object array</returns>
        Task<AccountHold[]> GetAccountHolds(string id);

        /// <summary>
        /// Place a basic market order trade
        /// </summary>
        /// <param name="side">Buy or sell</param>
        /// <param name="pair">Trading pair</param>
        /// <returns>OrderResponse object</returns>
        Task<OrderResponse> PlaceMarketOrder(SIDE side, string pair);

        /// <summary>
        /// Place a market order trade
        /// </summary>
        /// <param name="side">Buy or sell</param>
        /// <param name="pair">Trading pair</param>
        /// <param name="size">Amount in BTC</param>
        /// <param name="funds">Amount of quote currency to use</param>
        /// <returns>OrderResponse object</returns>
        Task<OrderResponse> PlaceMarketOrder(SIDE side, string pair, decimal size = 0.00000000M, decimal funds = 0.00000000M);

        /// <summary>
        /// Place a basic limit order trade
        /// </summary>
        /// <param name="side">Buy or sell</param>
        /// <param name="pair">Trading pair</param>
        /// <param name="price">Trading price</param>
        /// <param name="size">Amount in trade</param>
        /// <returns>OrderResponse object</returns>
        Task<OrderResponse> PlaceLimitOrder(SIDE side, string pair, decimal price, decimal size);

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
        Task<OrderResponse> PlaceLimitOrder(SIDE side, string pair, decimal price, decimal size, TimeInForce tif = TimeInForce.GTC, TradeCancelAfter tca = TradeCancelAfter.NONE);

        /// <summary>
        /// Place an order trade
        /// </summary>
        /// <param name="tradeParams">TradeParams for setting the trade</param>
        /// <returns>OrderResponse object</returns>
        Task<OrderResponse> PlaceTrade(TradeParams tradeParams);

        /// <summary>
        /// Place a stop limit trade
        /// </summary>
        /// <param name="tradeParams">GDAXStopLostParams for setting the SL</param>
        /// <returns>GDAXOrderResponse object</returns>
        Task<OrderResponse> PlaceStopOrder(StopType type, SIDE side, string pair, decimal price, decimal stop_price, decimal size);

        /// <summary>
        /// Place a stop limit trade
        /// </summary>
        /// <param name="tradeParams">GDAXStopLostParams for setting the SL</param>
        /// <returns>GDAXOrderResponse object</returns>
        Task<OrderResponse> PlaceStopOrder(StopLossParams tradeParams);

        /// <summary>
        /// Cancel an open order
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Boolean result of cancel request</returns>
        Task<bool> CancelOrder(string id);

        /// <summary>
        /// Cancel all open orders
        /// </summary>
        /// <returns>String array of canceled order ids</returns>
        Task<string[]> CancelOrders();

        /// <summary>
        /// Get all open orders
        /// </summary>
        /// <returns>OrderResponse array</returns>
        Task<OrderResponse[]> GetOrders();

        /// <summary>
        /// Get open orders
        /// </summary>
        /// <param name="pair">Trading pair (default = "")</param>
        /// <param name="status">Order status (default = all)</param>
        /// <returns>OrderResponse array</returns>
        Task<OrderResponse[]> GetOrders(string pair = "", OrderStatus status = OrderStatus.ALL);

        /// <summary>
        /// Get details of an order
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Order object</returns>
        Task<Order> GetOrder(string id);

        /// <summary>
        /// Get all fills
        /// </summary>
        /// <returns>Fill array</returns>
        Task<Fill[]> GetFills();

        /// <summary>
        /// Get user's 30-day trailing volume for all pairs
        /// </summary>
        /// <returns>TrailingVolume object array</returns>
        Task<TrailingVolume[]> GetTrailingVolume();

        /// <summary>
        /// Get available trading pairs
        /// </summary>
        /// <returns>ExchangeProduct object array</returns>
        Task<ExchangeProduct[]> GetTradingPairs();
    }
}
