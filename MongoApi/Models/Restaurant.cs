using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoApi.Models
{
    /// <summary>
    /// Represents a restaurant
    /// </summary>
    public class Restaurant
    {

        [BsonId]
        public ObjectId InternalId { get; set; }

        [BsonElement("address")]
        public Address Address { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("restaurant_id")]
        public string RestaurantId { get; set; }

        [BsonElement("grades")]
        public IEnumerable<GradeInstance> Grades { get; set; }

        [BsonElement("cuisine")]
        public string Cuisune { get; set; }

        [BsonElement("borough")]
        public string Borough { get; set; }
    }
}