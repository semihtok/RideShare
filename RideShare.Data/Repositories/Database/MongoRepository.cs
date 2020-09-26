using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using RideShare.Domain;

namespace RideShare.Data.Repositories.Database
{
    public class MongoRepository<T> where T:BaseModel
    {
        protected readonly IMongoCollection<T> Collection;

        protected MongoRepository(string connectionString, string db, string collection)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(db);
            Collection = database.GetCollection<T>(collection);
        }
        
        public T Find(string id)
        {
            var objectId = ObjectId.Parse(id);
            return Collection.Find<T>(m => m.Id == objectId).FirstOrDefault();
        }
        
        public List<T> FindAll()
        {
            return Collection.Find(c => true).ToList();
        }
        
        public void Create(T model)
        {
             Collection.InsertOne(model);
        }

        public bool Update(string id, T model)
        {
            var documentId = new ObjectId(id);
            var result = Collection.ReplaceOne(m => m.Id == documentId, model);
            return result.IsAcknowledged;
        }

        public void Delete(T model)
        {
            Collection.DeleteOne(m => m.Id == model.Id);
        }

        public void Delete(string id)
        {
            var docId = new ObjectId(id);
            Collection.DeleteOne(m => m.Id == docId);
        }
    }
    
}