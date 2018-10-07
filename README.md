# CoinbaseProApi.NetCore
.Net Core library for accessing the [Coinbase Pro](https://pro.coinbase.com/) api  
  
This library is available on NuGet for download: https://www.nuget.org/packages/CoinbaseProApi.NetCore/  
```
PM> Install-Package CoinbaseProApi.NetCore
```

  
To trade, log into your Coinbase Pro account and create an api key with trading permissions:  
Account -> API -> + New Api Key (with View & Trade permissions)  
Store your API Key, Secret Key, & API password  
  
Initialization:  
Non-secured endpoints only:  
```
var coinbasePro = new CoinbaseProClient();
```  
  
Secure & non-secure endpoints:  
```
var coinbasePro = new CoinbaseProClient("api-key", "api-secret", "api-password");  
```  
or
```
create config file config.json
{
  "apiKey": "api-key",
  "apiSecret": "api-secret",
  "apiPassword": "api-password",  
}
var coinbasePro = new CoinbaseProClient("/path-to/config.json");
```

Using an endpoint:  
```  
var balance = await coinbasePro.GetAccountsAsync();
```  
or  
```
var balance = coinbasePro.GetAccounts();
```

Non-secure endpoints:  
GetTradingPairs() | GetTradingPairsAsync() - Get available trading pairs  
GetOrderBook() | GetOrderBookAsync() - Get current order book  
GetTicker() | GetTickerAsync() - Get current ticker  
GetTrades() | GetTradesAsync() - Get recent trades  
GetHistoricRates() | GetHistoricRates() - Get historic rates (candlesticks)
GetStats() | GetStatsAsync() - Get 24hr stats  
GetCurrencies() | GetCurrenciesAsync() - Get all currencies  

Secure endpoints:  
GetAccounts() | GetAccountsAsync() - Get all current asset balances  
GetAccountBalance() | GetAccountBalanceAsync() - Get balance for one account  
GetAccountHistory() | GetAccountHistoryAsync() - Get history for user account  
GetAccountHolds() | GetAccountHoldsAsync() - Get holds on user account  
PlaceMarketOrder() | PlaceMarketOrderAsync() - Place a market order  
PlaceLimitOrder() | PlaceLimitOrderAsync() - Place a limit order  
PlaceTrade() | PlaceTradeAsync() - Place a trade  
PlaceStopOrder() | PlaceStopOrderAsync() - Place a stop order  
CancelOrder() | CancelOrderAsync() - Cancel an open order  
CancelOrders() | CancelOrdersAsync() - Cancel all open orders  
GetOrders() | GetOrdersAsync() - Get all open orders  
GetOrder() | GetOrderAsync() - Get an open order  
GetFills() | GetFillsAsync() - Get all fills  
GetTrailingVolume() | GetTrailingVolumeAsync() - Get 30-day trailing volume for all pairs  

ETH:  
0x3c8e741c0a2Cb4b8d5cBB1ead482CFDF87FDd66F  
BTC:  
1MGLPvTzxK9argeNRTHJ9EZ3WtGZV6nxit  
XLM:  
GA6JNJRSTBV54W3EGWDAWKPEGGD3QCXIGEHMQE2TUYXUKKTNKLYWEXVV  
