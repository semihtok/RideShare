using MongoDB.Driver;
using RideShare.Data.Repositories.Database;
using RideShare.Domain;

namespace RideShare.Data.Repositories
{
    public class TripRepository : MongoRepository<Trip>
    {
        public TripRepository(string connectionString, string db, string collection) : base(connectionString, db, collection)
        {
        }
    }
}