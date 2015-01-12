using System;
using System.Linq;
using System.Web.Mvc;
using Chart = System.Web.Helpers.Chart;


namespace BiofuelSouth.Controllers
{
    public class ChartController : Controller
    {
      

        public void GenerateChart(String cacheKey, Double[] data, String chartName)
        {
            
            int[] xValues = Enumerable.Range(1, data.Length).ToArray();
            var chart = new Chart(600, 300);
            chart.AddSeries(
                chartType: "Bar",
            
                name: chartName, 
                xValue: xValues,  
                yValues: data);
            chart.AddTitle(chartName);
            chart.SetXAxis("Year");
            chart.SetYAxis("Net Annual Production (tons)");

            chart.SaveToCache(cacheKey, 1);
            
         
        }

        public void ShowChart(string cacheKey)
        {
            Chart chart = Chart.GetFromCache(cacheKey);
            chart.Write();
        }


    }
}