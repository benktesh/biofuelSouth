using System;

namespace BiofuelSouth.Models
{
    public class County
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public string CountyCode { get; set; }
        public string GeoID { get; set; }
        public String State { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        
    }
}