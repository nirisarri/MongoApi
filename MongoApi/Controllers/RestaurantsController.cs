using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MongoApi.Models;

using MongoDB.Bson;
using MongoDB.Driver;
using BsonObjectId = Newtonsoft.Json.Bson.BsonObjectId;

namespace MongoApi.Controllers
{
    /// <summary>
    /// This is a manager for restaurant data.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class RestaurantsController : ApiController
    {
        private IMongoDatabase _database;

        private IMongoDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    var client = new MongoClient();
                    _database = client.GetDatabase("test");
                }

                return _database;
            }
        }

        /// <summary>
        /// Posts the specified restaurant.
        /// </summary>
        /// <param name="restaurant">The restaurant.</param>
        public async Task Post(Restaurant restaurant)
        {
            var document = CreateBson(restaurant) ?? GetSampleDocument();
            var collection = Database.GetCollection<Restaurant>("restaurants");
            await collection.InsertOneAsync(restaurant);
        }

        [HttpGet]
        [Route("api/Restaurants/{id}")]
        public async Task<Restaurant> GetById(long id)
        {
            var collection = Database.GetCollection<Restaurant>("restaurants");
            var filter = Builders<Restaurant>.Filter.Eq(x=>x.RestaurantId, id.ToString());
            var result = collection.Find(filter).ToList();
            return result.FirstOrDefault();
        }

        [HttpGet]
        [Route("api/Restaurants/Get/{id}")]
        public async Task<string> GetById2(long id)
        {
            var collection = Database.GetCollection<BsonDocument>("restaurants");
            var filter = Builders<BsonDocument>.Filter.Eq("restaurant_id", id.ToString());
            //                                                         restaurant_id
//            var filter = Builders<BsonDocument>.Filter.Eq("borough", "Manhattan");
//          var result = await collection.Find(filter).ToListAsync();
            var result = collection.Find(filter).FirstOrDefault();
            return result?.ToString();
        }

        public static BsonDocument CreateBson<T>(T source)
        {
            return BsonDocument.Create(source);
        }

        private static BsonDocument GetSampleDocument()
        {
            return new BsonDocument
            {
                {
                    "address", new BsonDocument
                    {
                        {"street", "2 ave"},
                        {"zipcode", "55444"},
                        {"building", "3660"},
                        {"coord", new BsonArray {-93.4, 45.003}}
                    }
                },
                {"borough", "Uptown"},
                {"cuisine", "Italian"},
                {
                    "grades", new BsonArray
                    {
                        new BsonDocument
                        {
                            {"date", DateTime.Parse("10/1/14 0:0:0 +0")},
                            {"grade", "A"},
                            {"score", 11}
                        },
                        new BsonDocument
                        {
                            {"date", DateTime.Parse("6/1/14 0:0:0 +0")},
                            {"grade", "B"},
                            {"score", 17}
                        }
                    }
                },
                {"name", "Vella"},
            };
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <returns>number of restaurants in the collection</returns>
        [HttpGet]
        [Route("api/Restaurants/Count")]
        public async Task<int> Count()
        {
            var collection = Database.GetCollection<BsonDocument>("restaurants");
            var filter = new BsonDocument();
            var count = 0;

            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    count += cursor.Current.Count();
                }
            }
            return count;
        }

        /// <summary>
        /// Gets the restaurants by borough.
        /// </summary>
        /// <param name="borough">The borough.</param>
        /// <returns>List of restaurants in a particular borough</returns>
        [HttpGet]
        [Route("api/Restaurants/ByBorough")]
        public async Task<IEnumerable<string>> GetByBorough(string borough)
        {
            var collection = Database.GetCollection<BsonDocument>("restaurants");
            var filter = Builders<BsonDocument>.Filter.Eq("borough", borough);
            var result = await collection.Find(filter).ToListAsync();

            return result.Select(x => x["restaurant_id"].AsString);
        }

        /// <summary>
        /// Gets the boroughs.
        /// </summary>
        /// <returns>List of Boroughs</returns>
        [HttpGet]
        [Route("api/Boroughs")]
        public async Task<IEnumerable<string>> GetBoroughs()
        {
            var collection = Database.GetCollection<BsonDocument>("restaurants");
            var filter = new BsonDocument();
            FieldDefinition<BsonDocument, string> field = "borough";
            var result = await collection.Distinct(field, filter).ToListAsync();
            return result;
        }
    }
}