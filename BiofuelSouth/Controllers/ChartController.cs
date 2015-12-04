using System;
using System.Linq;
using System.Web.Mvc;
using BiofuelSouth.Models;
using Chart = System.Web.Helpers.Chart;


namespace BiofuelSouth.Controllers
{
    public class ChartController : Controller
    {
        private int ChartCacheMinute = 5; 
      public void GenerateChart(string cacheKey, Double[] data, String chartName)
        {
            var xValues = Enumerable.Range(1, data.Length).ToArray();
            var chart = new Chart(600, 300);
            chart.AddSeries(
                chartType: "Column",
                name: chartName,
                xValue: xValues,
                yValues: data);
            chart.SetXAxis("Year", 0, data.Length + .75);
            chart.AddTitle(chartName);
            chart.SetYAxis("Annual Production (tons)");
            chart.SaveToCache(cacheKey, ChartCacheMinute);
        }

        public void GenerateColumnChart(string cacheKey, Double[] data, String chartName, string xLabel, string yLabel)
        {
            var xValues = Enumerable.Range(1, data.Length).ToArray();
            var chart = new Chart(600, 300);
            chart.AddSeries(
                chartType: "Column",
                name: chartName,
                xValue: xValues,
                
            yValues: data);
            chart.SetXAxis(xLabel + " ", 0, data.Length + .75);
            chart.AddTitle(chartName);
            chart.SetYAxis(yLabel + " ", Math.Round(data.Min(),0), Math.Round(data.Max(),0));
            chart.SaveToCache(cacheKey, ChartCacheMinute);
        }

        public void GenerateCostRevenueChart(String cacheKey, Input ip, String chartName)
        {
        
            var data = ip.GetRevenues().Select(m => m.TotalRevenue).ToArray();
            var xValues = Enumerable.Range(1, data.Length).ToArray();

            var chart = new Chart(600, 300);
            chart.AddSeries(
                chartType: "Line",
                name: "Revenue",
                xValue: xValues,
                yValues: data);
            
            data = ip.GetExpenditures().Select(m => m.TotalExpenses).ToArray();
            chart.AddSeries(
                chartType: "Line",
                
                name: "Cost",
                
                xValue: xValues,
                yValues: data);


            chart.AddLegend("Legend");
            chart.SetXAxis("Year", 0, data.Length + .75);
            chart.AddTitle(chartName);
            chart.SetYAxis("$ ");
            chart.SaveToCache(cacheKey, ChartCacheMinute);
        }


        public void GetCostRevenueChart(string cachekey, Input ip, String chartName)
        {
            var xValues = Enumerable.Range(1, ip.General.ProjectLife.GetValueOrDefault()).ToArray();
            var revenues = ip.GetRevenues().Select(m => m.TotalRevenue).ToArray();
            var chart = new Chart(600, 300);
            chart.AddSeries(
                chartType: "Line",
                name: chartName,
                xValue: xValues,
                yValues: revenues);

            chart.SetXAxis("Year", 0, xValues.Length + .75);
            chart.AddTitle(chartName);
            chart.SetYAxis("Cost and Revenue (Actual $)");
            chart.SaveToCache(cachekey, ChartCacheMinute);
        }

        public void ShowChart(string cacheKey)
        {
            var chart = Chart.GetFromCache(cacheKey);
            chart.Write();
        }


    }
}