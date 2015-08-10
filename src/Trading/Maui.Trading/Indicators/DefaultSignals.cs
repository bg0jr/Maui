using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maui.Trading.Model;

namespace Maui.Trading.Indicators
{
    public class NoSignal : Signal
    {
        public NoSignal()
        {
            Type = SignalType.None;
            Strength = Percentage.Zero;
            Quality = Percentage.Zero;
        }

        protected override Signal CreateTemplate()
        {
            return new NoSignal();
        }

        public override Signal Weight( double factor )
        {
            return new NoSignal();
        }
    }

    public class BuySignal : Signal
    {
        public BuySignal()
            : this( 50 )
        {
        }

        public BuySignal( int strength )
            : this( strength, 100 )
        {
        }

        public BuySignal( int strength, int quality )
            : base( SignalType.Buy, new Percentage( strength ), new Percentage( quality ) )
        {
        }

        protected override Signal CreateTemplate()
        {
            return new BuySignal();
        }
    }

    public class NeutralSignal : Signal
    {
        public NeutralSignal()
            : this( 50 )
        {
        }

        public NeutralSignal( int strength )
            : this( strength, 100 )
        {
        }

        public NeutralSignal( int strength, int quality )
            : base( SignalType.Neutral, new Percentage( strength ), new Percentage( quality ) )
        {
        }

        protected override Signal CreateTemplate()
        {
            return new NeutralSignal();
        }
    }

    public class SellSignal : Signal
    {
        public SellSignal()
            : this( 50 )
        {
        }

        public SellSignal( int strength )
            : this( strength, 100 )
        {
        }

        public SellSignal( int strength, int quality )
            : base( SignalType.Sell, new Percentage( strength ), new Percentage( quality ) )
        {
        }

        protected override Signal CreateTemplate()
        {
            return new SellSignal();
        }
    }
}
