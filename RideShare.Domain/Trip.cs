using System;
using System.Collections.Generic;

namespace RideShare.Domain
{
    public class Trip : BaseModel
    {
        public string Description { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal OriginLocationX { get; set; }
        public decimal OriginLocationY { get; set; }
        public decimal DestinationLocationX { get; set; }
        public decimal DestinationLocationY { get; set; }
        public DateTime TripTime { get; set; }
        public User Driver { get; set; }
        public List<User> Riders { get; set; }
    }
}