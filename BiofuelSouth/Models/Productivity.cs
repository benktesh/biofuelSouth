using System;

namespace BiofuelSouth.Models
{
    public class Productivity
    {
        public int Id { get; set; }
        public int GeoId { get; set; }
        public String CropType { get; set; }
        public Double Yield { get; set; }
        public Double Cost { get; set; }
    }
}