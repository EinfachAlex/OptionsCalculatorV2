namespace OptionsCalculatorV2.BlackScholes
{
    public class PutStrategy : BlackScholesStrategy
    {
        public override double getDelta(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield)
        {
            throw new System.NotImplementedException();
        }

        public override double getGamma(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield)
        {
            throw new System.NotImplementedException();
        }

        public override double getTheta(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield)
        {
            throw new System.NotImplementedException();
        }

        public override double getOmega(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield)
        {
            throw new System.NotImplementedException();
        }

        public override double getCallPrice(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double historicalVolatility, double dividendYield)
        {
            throw new System.NotImplementedException();
        }

        public override double getIV(double underlyingPrice, double strikePrice, double YTE, double riskFreeRate, double marketPrice, double dividendYield)
        {
            throw new System.NotImplementedException();
        }
    }
}