using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OptionsCalculatorV2.BlackScholes;
using System.Runtime.InteropServices;

namespace OptionsCalculatorV2
{
    public partial class Form1 : Form
    {
        #region init

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //AllocConsole();
            this.optionsCalculator = OptionsCalculator.getInstance();
        }

        #endregion init

        private OptionsCalculator optionsCalculator;

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.optionsCalculator.onNewCalculation(underlyingPriceInput.Text, strikePriceInput.Text, dteInput.Text, historicalVolatilityInput.Text, riskFreeRateInput.Text, dividendYieldInput.Text, ratioInput.Text, bidPriceInput.Text, new CallStrategy());

            refreshGUI();
        }
        
        

        private void refreshGUI()
        {
            if (!areOptionInputsFilled()) return;

            clearCharts();

            drawGreeksChart();

            drawOmegaChart();

            drawThetaDecayChart();

            this.IVOutput.Text = (optionsCalculator.getIV(double.Parse(bidPriceInput.Text)) * 100).ToString();
        }

        private bool areOptionInputsFilled()
        {
            return !string.IsNullOrWhiteSpace(underlyingPriceInput.Text) &&
                   !string.IsNullOrWhiteSpace(strikePriceInput.Text) &&
                   !string.IsNullOrWhiteSpace(dteInput.Text) &&
                   !string.IsNullOrWhiteSpace(historicalVolatilityInput.Text) &&
                   !string.IsNullOrWhiteSpace(riskFreeRateInput.Text) &&
                   !string.IsNullOrWhiteSpace(dividendYieldInput.Text) &&
                   !string.IsNullOrWhiteSpace(ratioInput.Text);
        }

        #region charts

        private void drawOmegaChart()
        {
            ChartSize chartSize = getChartSize();
            
            for (int price = (int) chartSize.chartStart; price < chartSize.chartEnd; price++)
            {
                double omega = optionsCalculator.getOmega(price);

                omegaChart.Series[0].Points.AddXY(price, omega);
            }
        }

        private void drawThetaDecayChart()
        {
            double optionPrice = optionsCalculator.getCallPrice();

            for (int daysLeft = (int) optionsCalculator.DTE; daysLeft > 0; daysLeft--)
            {
                if (daysLeft == optionsCalculator.DTE) continue;
                
                double theta = optionsCalculator.getTheta(0, (double) daysLeft);

                optionPrice += theta;

                thetaDecay.Series[0].Points.AddXY(-daysLeft, optionPrice);
            }
        }

        private void drawGreeksChart()
        {
            ChartSize chartSize = getChartSize();
            
            for (int price = (int) chartSize.chartStart; price < chartSize.chartEnd; price++)
            {
                double delta = optionsCalculator.getDelta(price);
                double gamma = optionsCalculator.getGamma(price);
                double theta = optionsCalculator.getTheta(price);

                chart1.Series[0].Points.AddXY(price, gamma);
                chart1.Series[1].Points.AddXY(price, delta);
                chart1.Series[2].Points.AddXY(price, theta);

                Console.WriteLine($"Price: {price} / Delta: {delta} / Gamma: {gamma} / Theta: {theta}");
            }
        }

        private void clearCharts()
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();

            omegaChart.Series[0].Points.Clear();

            thetaDecay.Series[0].Points.Clear();
        }

        private ChartSize getChartSize()
        {
            double chartStart;
            double chartEnd;

            if (optionsCalculator.strikePrice > optionsCalculator.underlyingPrice)
            {
                chartEnd = optionsCalculator.strikePrice;
                chartStart = optionsCalculator.underlyingPrice;
            }
            else
            {
                chartStart = optionsCalculator.strikePrice;
                chartEnd = optionsCalculator.underlyingPrice;
            }

            ChartSize chartSize = new ChartSize(chartStart, chartEnd);

            return chartSize;
        }

        #endregion charts
        
        #region inputChanged
        private void onOptionTextInputChanged(object sender, EventArgs e)
        {
            if (areOptionInputsFilled())
            {
                this.optionsCalculator.onNewCalculation(underlyingPriceInput.Text, strikePriceInput.Text, dteInput.Text, historicalVolatilityInput.Text, riskFreeRateInput.Text, dividendYieldInput.Text, ratioInput.Text, bidPriceInput.Text, new CallStrategy());
                
                refreshGUI();
            }
        }
        #endregion
        
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        
    }
}