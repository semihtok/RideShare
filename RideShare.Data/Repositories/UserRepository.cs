using MongoDB.Bson;
using MongoDB.Driver;
using RideShare.Data.Repositories.Database;
using RideShare.Domain;

namespace RideShare.Data.Repositories
{
    public class UserRepository : MongoRepository<User>
    {
        public UserRepository(string connectionString, string db, string collection) : base(connectionString, db, collection)
        {
        }
        
        public User Authenticate(string phone, string password)
        {
            return Collection.Find(m => m.Phone == phone && m.Password == password).Single();
        }
        
        public User Find(string phone)
        {
            return Collection.Find(m => m.Phone == phone).Single();
        }
        
    }
}