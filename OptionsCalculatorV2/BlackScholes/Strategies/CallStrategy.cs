using System;

namespace OptionsCalculatorV2.BlackScholes
{
    public class CallStrategy : BlackScholesStrategy
    {
        public override double getDelta(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield)
        {
            double delta = NormSDist.N(calculateDOne(underlyingPrice, strikePrice, YTE, riskFreeRate, historicalVolatility, dividendYield));

            return delta;
        }

        public override double getGamma(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield)
        {
            double gamma = calculateNdOne(underlyingPrice, strikePrice, YTE, riskFreeRate, historicalVolatility, dividendYield);

            return gamma;
        }

        public override double getTheta(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield)
        {
            double ndOne = calculateNdOne(underlyingPrice, strikePrice, YTE, riskFreeRate, historicalVolatility, dividendYield);
            double ndTwo = calculateNdTwo(underlyingPrice, strikePrice, YTE, riskFreeRate, historicalVolatility, dividendYield);

            double theta = -(underlyingPrice * historicalVolatility * ndOne) / (2 * Math.Sqrt(YTE)) - riskFreeRate * strikePrice * Math.Pow(-riskFreeRate * (YTE), 2) * ndTwo;

            return theta / 365;
        }

        public override double getOmega(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield)
        {
            double delta = getDelta(underlyingPrice, strikePrice, YTE, riskFreeRate, historicalVolatility, dividendYield);
            double callPrice = getCallPrice(underlyingPrice, strikePrice, YTE, riskFreeRate, historicalVolatility, dividendYield);

            double omega = (underlyingPrice * delta) / callPrice;

            return omega;
        }

        public override double getCallPrice(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield)
        {
            double callOption = Math.Exp(-dividendYield * YTE) * underlyingPrice * NormSDist.N(calculateDOne(underlyingPrice, strikePrice, YTE, riskFreeRate, historicalVolatility, dividendYield)) - strikePrice * Math.Exp(-riskFreeRate * YTE) *
                NormSDist.N(calculateDOne(underlyingPrice, strikePrice, YTE, riskFreeRate, historicalVolatility, dividendYield) - historicalVolatility * Math.Sqrt(YTE));

            return callOption;
        }

        public override double getIV(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double marketPrice, double dividendYield)
        {
            double high = 5;
            double low = 0;

            while ((high - low) > 0.0001)
            {
                double callPrice = getCallPrice(underlyingPrice, strikePrice, YTE, riskFreeRate, (high + low) / 2, dividendYield);

                if (callPrice > marketPrice)
                {
                    high = (high + low) / 2;
                }
                else
                {
                    low = (high + low) / 2;
                }
            }

            double impliedVolatility = (high + low) / 2;

            return impliedVolatility;
        }
    
}
}