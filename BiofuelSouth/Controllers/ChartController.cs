using System;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using BiofuelSouth.Models;
using Microsoft.Ajax.Utilities;
using Chart = System.Web.Helpers.Chart;


namespace BiofuelSouth.Controllers
{
    public class ChartController : Controller
    {
      public void GenerateChart(string cacheKey, Double[] data, String chartName)
        {
            int[] xValues = Enumerable.Range(1, data.Length).ToArray();
            var chart = new Chart(600, 300);
            chart.AddSeries(
                chartType: "Column",
                name: chartName,
                xValue: xValues,
                yValues: data);
            chart.SetXAxis("Year", 0, data.Length + .75);
            chart.AddTitle(chartName);
            chart.SetYAxis("Annual Production (tons)  ");
            chart.SaveToCache(cacheKey, 1);
        }

        public void GenerateCostRevenueChart(String cacheKey, Input ip, String chartName)
        {
        
            var data = ip.GetRevenues().Select(m => m.TotalRevenue).ToArray();
            int[] xValues = Enumerable.Range(1, data.Length).ToArray();

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
            chart.SetYAxis("$ (Actual Dollars) ");
            chart.SaveToCache(cacheKey, 1);
        }


        public void GetCostRevenueChart(string cachekey, Input ip, String chartName)
        {
            int[] xValues = Enumerable.Range(1, ip.General.ProjectLife).ToArray();
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
            chart.SaveToCache(cachekey, 5);
        }

        public void ShowChart(string cacheKey)
        {
            var chart = Chart.GetFromCache(cacheKey);
            chart.Write();
        }


    }
}