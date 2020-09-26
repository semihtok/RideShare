using System;
using System.Collections.Generic;

namespace RideShare.Domain
{
    public class User : BaseModel
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public List<Trip> Trips { get; set; }
        public int AvailableSeats { get; set; }
    }
}