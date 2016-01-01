using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BiofuelSouth.Models;
using Chart = System.Web.Helpers.Chart;


namespace BiofuelSouth.Controllers
{
    public class ChartController : Controller
    {
        private int ChartCacheMinute = 5;
        public void GenerateChart(string cacheKey, decimal[] data, String chartName)
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

        public void GenerateColumnChart(string cacheKey, decimal[] data, String chartName, string xLabel, string yLabel)
        {
            var xValues = Enumerable.Range(1, data.Length).ToArray();
            var chart = new Chart(600, 300);
            chart.AddSeries(
                chartType: "Column",
                name: chartName,
                xValue: xValues,

            yValues: data);

            chart.AddSeries(chartType: "line", yValues: Enumerable.Repeat(0, data.Length).ToArray());
            chart.SetXAxis(xLabel + " ", 0, data.Length + .75);

            chart.AddTitle(chartName);
            //chart.SetYAxis(yLabel + " ", Math.Round(data.Min(),0), Math.Round(data.Max(),0));
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="dataMember"></param>
        /// <param name="labels">Label corresponding to series names for datamember</param>
        /// <param name="chartName"></param>
        /// <param name="yLabel"></param>
        /// <param name="xLabel"></param>
        public void GenerateLineGraphs(String cacheKey, List<List<decimal>> dataMember, 
            List<string> labels, string chartName, string yLabel = "$ ", string xLabel = "Year")
        {
            yLabel = yLabel + " ";
            var dataItems = dataMember.Count;
            if (dataItems == 0)
                return; 

            var xAxisLength = dataMember.First().Count;
            if (xAxisLength == 0)
                return;

            var chart = new Chart(600, 300);
            var xValues = Enumerable.Range(1, xAxisLength).ToArray();
            for (int i = 0; i < dataItems; i++)
            {
                var data = dataMember[i].ToArray();
                chart.AddSeries(
                    chartType: "Line",
                    name: labels[i],
                    xValue: xValues,
                    yValues: data);
            }
            
            chart.AddLegend("Legend");
            chart.SetXAxis(xLabel, 0, xAxisLength + .75);
            chart.AddTitle(chartName);
            chart.SetYAxis(yLabel);
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