using System;
using System.Collections.Generic;  
using System.Linq;  
using System.Text;  
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HandApi.Controllers
{
    [ApiController]
    [Route("usedcars/[controller]")]
    public class UsedCarsController : ControllerBase
    {
        [HttpGet]
        public List<UsedCar> Get()
        {
            List<UsedCar> retunable = new List<UsedCar>();
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase database = client.GetDatabase("UsedCars");
            var collection = database.GetCollection<Entity>("Cars");
            var resultDoc = collection.Find(new BsonDocument()).ToList();
            foreach( var item in resultDoc){
                retunable.Add(item);
            }
            return retunable;
        }

        [HttpPost]
        static async void Insert(UsedCar car)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase database = client.GetDatabase("UsedCars");
            var collection = database.GetCollection<Entity>("Cars");
            await collection.InsertOneAsync(new Entity
            {
                _id = ObjectId.GenerateNewId(),
                Model = car.Model,
                Description = car.Description,
                Year = car.Year,
                Brand = car.Brand,
                Kilometers = car.Kilometers,
                Predicate = car.Price
            });
        }

        [HttpPost]
        public UsedCar Delete(UsedCar car)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase database = client.GetDatabase("UsedCars");
            var collection = database.GetCollection<Entity>("Cars");
            BsonDocument findCarDoc = new BsonDocument(new BsonElement(car));

            return collection.FindOneAndDelete(findCarDoc);
        }
    }
}