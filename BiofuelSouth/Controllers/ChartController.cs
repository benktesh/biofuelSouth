using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using System.Xml.Schema;
using Chart = System.Web.Helpers.Chart;


namespace BiofuelSouth.Controllers
{
    public class ChartController : Controller
    {
        // GET: Chart
        public ActionResult Index()
        {
            return View();
        }


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
            chart.SetXAxis(
                title: "Year");
            chart.SetYAxis(
                title: "Net Annual Production (tons)");

            chart.SaveToCache(cacheKey, 1);
            
         
        }

        public void ShowChart(string cacheKey)
        {
            Chart chart = Chart.GetFromCache(cacheKey);
            chart.Write("jpeg");
        }


    }
}