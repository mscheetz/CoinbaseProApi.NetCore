using System;
using System.Collections.Generic;
using System.Text;

namespace CoinbaseProApi.NetCore.Entities
{
    public enum TimeInForce
    {
        GTC,
        GTT,
        IOC,
        FOK
    }

    public enum SIDE
    {
        NONE,
        BUY,
        SELL
    }

    public enum TradeType
    {
        LIMIT,
        MARKET
    }

    public enum StopType
    {
        NONE,
        LIMIT,
        ENTRY
    }

    public enum TradeCancelAfter
    {
        NONE,
        MIN,
        HOUR,
        DAY
    }

    public enum OrderStatus
    {
        ALL,
        OPEN,
        PENDING,
        ACTIVE
    }

    public enum Granularity
    {
        OneM,
        FiveM,
        FifteenM,
        OneH,
        SixH,
        OneD
    }
}
