using CoinbaseProApi.NetCore.Core;
using CoinbaseProApi.NetCore.Data;
using CoinbaseProApi.NetCore.Data.Interfaces;
using CoinbaseProApi.NetCore.Entities;
using DateTimeHelpers;
using FileRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CoinbaseProApi.NetCore.Tests
{
    public class CoinbaseProRepositoryTests : IDisposable
    {
        private ApiInformation _exchangeApi = null;
        private ICoinbaseProRepository _repo;
        private ICoinbaseProRepository _repoAuth;
        private string configPath = "config.json";
        private Helper helper = new Helper();
        private DateTimeHelper dtHelper = new DateTimeHelper();

        public CoinbaseProRepositoryTests()
        {
            IFileRepository _fileRepo = new FileRepository.FileRepository();
            if (_fileRepo.FileExists(configPath))
            {
                _exchangeApi = _fileRepo.GetDataFromFile<ApiInformation>(configPath);
            }
            if (_exchangeApi != null && !string.IsNullOrEmpty(_exchangeApi.apiKey))
            {
                _repoAuth = new CoinbaseProRepository(_exchangeApi, true);
            }
            _repo = new CoinbaseProRepository();
        }

        public void Dispose()
        {

        }

        [Fact]
        public void GetAccounts_Test()
        {
            // act
            var account = _repoAuth.GetAccounts().Result;

            // assert
            Assert.NotNull(account);
        }

        [Fact]
        public void GetAccount_Test()
        {
            // arrange
            var accountId = "e5607305-b63f-4746-ab14-c2cb613b9b8d";

            //act
            var account = _repoAuth.GetAccount(accountId).Result;

            // assert
            Assert.NotNull(account);
        }

        [Fact]
        public void GetAccountBalance_Test()
        {
            // act
            var account = _repoAuth.GetAccounts().Result;

            // arrange
            var id = account[0].id;

            // act
            var balance = _repoAuth.GetAccountBalance(id).Result;

            // assert
            Assert.NotNull(balance);
        }

        [Fact]
        public void GetAccountHistory_Test()
        {
            // act
            var account = _repoAuth.GetAccounts().Result;

            // arrange
            var id = account[0].id;

            // act
            var history = _repoAuth.GetAccountHistory(id).Result;

            // assert
            Assert.NotNull(history);
        }

        [Fact]
        public void GetAccountHolds_Test()
        {
            // act
            var account = _repoAuth.GetAccounts().Result;

            // arrange
            var id = account[0].id;

            // act
            var holds = _repoAuth.GetAccountHolds(id).Result;

            // assert
            Assert.NotNull(holds);
        }

        [Fact]
        public void PlaceMarketOrder_Test1()
        {
            var side = SIDE.SELL;
            var pair = "BTCUSD";
            var size = 0.5M;

            // act
            var order = _repoAuth.PlaceMarketOrder(side, pair, size).Result;

            // assert
            Assert.NotNull(order);
        }

        [Fact]
        public void PlaceMarketOrder_Test2()
        {
            var side = SIDE.SELL;
            var pair = "BTCUSD";
            var size = 0.0M;
            var funds = 1000.0M;

            // act
            var order = _repoAuth.PlaceMarketOrder(side, pair, size, funds).Result;

            // assert
            Assert.NotNull(order);
        }

        [Fact]
        public void PlaceLimitOrder_Test1()
        {
            var side = SIDE.SELL;
            var pair = "BTCUSD";
            var price = 7500.00M;
            var size = 0.5M;

            // act
            var order = _repoAuth.PlaceLimitOrder(side, pair, price, size).Result;

            // assert
            Assert.NotNull(order);
        }

        [Fact]
        public void PlaceLimitOrder_Test2()
        {
            var side = SIDE.SELL;
            var pair = "BTCUSD";
            var price = 10000.00M;
            var size = 2.0M;
            var tif = TimeInForce.GTC;

            // act
            var order = _repoAuth.PlaceLimitOrder(side, pair, price, size, tif).Result;

            // assert
            Assert.NotNull(order);
        }

        [Fact]
        public void PlaceStopLoss_Test()
        {
            var type = StopType.LIMIT;
            var side = SIDE.SELL;
            var pair = "BTCUSD";
            var price = 6100.00M;
            var stop_price = 6100.00M;
            var size = 0.5M;

            // act
            var stopLoss = _repoAuth.PlaceStopOrder(type, side, pair, price, stop_price, size).Result;

            // assert
            Assert.NotNull(stopLoss);
        }

        [Fact]
        public void CancelOrder_Test()
        {
            var id = "2852d1cf-ad53-46b8-870e-cfe1ffcb0fde";

            // act
            var order = _repoAuth.CancelOrder(id).Result;

            // assert
            Assert.NotNull(order);
        }

        [Fact]
        public void CancelOrders_Test()
        {
            // act
            var orders = _repoAuth.CancelOrders().Result;

            // assert
            Assert.NotNull(orders);
        }

        [Fact]
        public void GetOrders_Test()
        {
            // arrange
            var pair = "BTCUSD";

            // act
            var orders = _repoAuth.GetOrders(pair).Result;

            // assert
            Assert.NotNull(orders);
        }

        [Fact]
        public void GetOrder_Test()
        {
            // arrange
            var id = "443501";

            // act
            var order = _repoAuth.GetOrder(id).Result;

            // assert
            Assert.NotNull(order);
        }

        [Fact]
        public void GetFills_Test()
        {
            // act
            var fills = _repoAuth.GetFills().Result;

            // assert
            Assert.NotNull(fills);
        }

        [Fact]
        public void GetTrailingVolume_Test()
        {
            // act
            var volume = _repoAuth.GetTrailingVolume().Result;

            // assert
            Assert.NotNull(volume);
        }

        [Fact]
        public void GetTradingPairs_Test()
        {
            // act
            var pairs = _repo.GetTradingPairs().Result;

            // assert
            Assert.NotNull(pairs);
        }

        [Fact]
        public void GetOrderBook_Test_Level1()
        {
            // arrange
            var pair = "BTCUSD";
            var level = 1;

            // act
            var book = _repo.GetOrderBook(pair, level).Result;

            // assert
            Assert.NotNull(book);
        }

        [Fact]
        public void GetOrderBook_Test_Level2()
        {
            // arrange
            var pair = "BTCUSD";

            // act
            var book = _repo.GetOrderBook(pair).Result;

            // assert
            Assert.NotNull(book);
        }

        [Fact]
        public void GetOrderBook_Test_Level3()
        {
            // arrange
            var pair = "BTCUSD";
            var level = 3;

            // act
            var book = _repo.GetOrderBook(pair, level).Result;

            // assert
            Assert.NotNull(book);
        }

        [Fact]
        public void GetTicker_Test()
        {
            // arrange
            var pair = "BTCUSD";

            // act
            var ticker = _repo.GetTicker(pair).Result;

            // assert
            Assert.NotNull(ticker);
        }

        [Fact]
        public void GetTrades_Test()
        {
            // arrange
            var pair = "BTCUSD";

            // act
            var trades = _repo.GetTrades(pair).Result;

            // assert
            Assert.NotNull(trades);
        }

        [Fact]
        public void GetHistoricRatesTest_NoTimes()
        {
            // arrange
            var pair = "BTCUSD";
            var gran = Granularity.FiveM;

            // act
            var rates = _repo.GetHistoricRates(pair, gran, 20).Result;

            // assert
            Assert.NotNull(rates);
        }

        [Fact]
        public void GetHistoricRatesTestII_NoTimes()
        {
            // arrange
            var pair = "ETHUSD";
            var gran = Granularity.OneD;
            var dtHelper = new DateTimeHelper();
            // act
            var rates = _repo.GetHistoricRates(pair, gran, 300).Result;

            // assert
            Assert.NotNull(rates);

            var firstTS = rates.Min(r => r.time);
            var firstDate = dtHelper.UnixTimeToUTC(firstTS);
            rates = _repo.GetHistoricRates(pair, firstDate, gran, 300).Result;

            if(rates.Any())
            {
                Assert.NotNull(rates);
            }
            firstTS = rates.Min(r => r.time);
            firstDate = dtHelper.UnixTimeToUTC(firstTS);
            rates = _repo.GetHistoricRates(pair, firstDate, gran, 300).Result;

            if (rates.Any())
            {
                Assert.NotNull(rates);
            }
            firstTS = rates.Min(r => r.time);
            firstDate = dtHelper.UnixTimeToUTC(firstTS);
            rates = _repo.GetHistoricRates(pair, firstDate, gran, 300).Result;

            if (rates.Any())
            {
                Assert.NotNull(rates);
            }
        }

        [Fact]
        public void GetHistoricRatesTest_EndTime()
        {
            // arrange
            var pair = "BTCUSD";
            var gran = Granularity.FiveM;
            var end = DateTime.UtcNow.AddMinutes(-20);

            // act
            var rates = _repo.GetHistoricRates(pair, end, gran, 20).Result;
            var firstTime = dtHelper.UnixTimeToUTC(rates[0].time);
            var LastTime = dtHelper.UnixTimeToUTC(rates[rates.Length - 1].time);

            // assert
            Assert.NotNull(rates);
        }

        [Fact]
        public void GetCurrenciesTest()
        {
            // act
            var currencies = _repo.GetCurrencies().Result;

            // assert
            Assert.NotNull(currencies);
        }
    }
}
