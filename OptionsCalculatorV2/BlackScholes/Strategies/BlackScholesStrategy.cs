using System;

namespace OptionsCalculatorV2.BlackScholes
{
    public abstract class BlackScholesStrategy
    {
        protected double calculateDOne(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield)
        {
            double dOne = (Math.Log(underlyingPrice / strikePrice) + (riskFreeRate - dividendYield + 0.5 * Math.Pow(historicalVolatility, 2)) * YTE) / (historicalVolatility * Math.Sqrt(YTE));

            return dOne;
        }

        protected double calculateNdOne(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield)
        {
            double dOne = calculateDOne(underlyingPrice, strikePrice, YTE, riskFreeRate, historicalVolatility, dividendYield);

            double NdOne = Math.Exp(-(Math.Pow(dOne, 2) / 2)) / (Math.Sqrt(2 * Math.PI));

            return NdOne;
        }

        protected double calculateDTwo(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield)
        {
            double DOne = calculateDOne(underlyingPrice, strikePrice, YTE, riskFreeRate, historicalVolatility, dividendYield) - historicalVolatility * Math.Sqrt(YTE);

            return DOne;
        }

        protected double calculateNdTwo(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield)
        {
            double NdTwo = NormSDist.N(calculateDTwo(underlyingPrice, strikePrice, YTE, riskFreeRate, historicalVolatility, dividendYield));

            return NdTwo;
        }
        public abstract double getDelta(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield);


        public abstract double getGamma(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield);


        public abstract double getTheta(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield);


        public abstract double getOmega(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield);


        public abstract double getCallPrice(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield);


        public abstract double getIV(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double marketPrice, double dividendYield);
    }
}