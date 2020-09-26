using System;
using MongoDB.Bson;

namespace RideShare.Domain
{
    public class BaseModel
    {
        public ObjectId Id { get; set; }
        private DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}