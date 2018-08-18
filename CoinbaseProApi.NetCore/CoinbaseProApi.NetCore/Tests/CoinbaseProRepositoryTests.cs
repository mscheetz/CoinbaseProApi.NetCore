using CoinbaseProApi.NetCore.Core;
using CoinbaseProApi.NetCore.Data;
using CoinbaseProApi.NetCore.Data.Interfaces;
using CoinbaseProApi.NetCore.Entities;
using DateTimeHelpers;
using FileRepository;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoinbaseProApi.NetCore.Tests
{
    public class CoinbaseProRepositoryTests : IDisposable
    {
        private ApiInformation _exchangeApi = null;
        private ICoinbaseProRepository _repo;
        private ICoinbaseProRepository _repoAuth;
        private string configPath = "";
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
