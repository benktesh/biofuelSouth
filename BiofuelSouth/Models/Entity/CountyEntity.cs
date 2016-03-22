using System;

namespace BiofuelSouth.Models.Entity
{
    public class CountyEntity
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public string CountyCode { get; set; }
        public string GeoId { get; set; }
        public String State { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        
    }
}