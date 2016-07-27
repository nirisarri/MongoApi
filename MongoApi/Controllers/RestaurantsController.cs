using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
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
            var collection = Database.GetCollection<Restaurant>("restaurants");
            await collection.InsertOneAsync(restaurant);
        }

        [HttpGet]
        [Route("api/Restaurants")]
        public async Task<IEnumerable<Restaurant>> GetByName(string name)
        {
            var collection = Database.GetCollection<Restaurant>("restaurants");
            var filter = Builders<Restaurant>.Filter.Regex(x=>x.Name, new BsonRegularExpression(name+".*", "i"));
            var result = await collection.Find(filter).ToListAsync();
            return RestUtil.Response(result);
        }

        [HttpGet]
        [Route("api/Restaurants/{id}")]
        public async Task<Restaurant> GetById(long id)
        {
            var collection = Database.GetCollection<Restaurant>("restaurants");
            var filter = Builders<Restaurant>.Filter.Eq(x=>x.RestaurantId, id.ToString());
            var result = await collection.Find(filter).FirstOrDefaultAsync();
            return RestUtil.Response(result);
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