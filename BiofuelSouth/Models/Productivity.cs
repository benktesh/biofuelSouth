using System;

namespace BiofuelSouth.Models
{
    public class Productivity
    {
        public int Id { get; set; }
        public Int32 GeoId { get; set; }
        public String CropType { get; set; }
        public Double Yield { get; set; }
        public Double Cost { get; set; }
    }
}