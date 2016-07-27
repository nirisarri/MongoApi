using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoApi.Models
{
    public class GradeInstance
    {

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("grade")]
        public char Grade { get; set; }

        [BsonElement("score")]
        public int Score { get; set; }
    }
}