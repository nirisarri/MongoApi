using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoApi.Models
{
    public class Address
    {

        [BsonElement("building")]
        public int Building { get; set; }

        [BsonElement("coord")]
        public double[] Coord { get; set; }

        [BsonElement("street")]
        public string Street { get; set; }

        [BsonElement("zipcode")]
        public string ZipCode { get; set; }

        public double? Lat => Coord?[0];
        public double? Long => Coord?[1];

    }
}