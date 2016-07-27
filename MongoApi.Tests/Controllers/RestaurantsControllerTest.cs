
using FluentAssertions;
using FluentAssertions.Equivalency;
using MongoApi.Controllers;
using MongoDB.Bson;
using NUnit.Framework;

namespace MongoApi.Tests.Controllers
{
    [TestFixture]
    public class RestaurantsControllerTest
    {
        [Test]
        public void CreateBson_should_take_bson_and_make_an_object()
        {
            BsonDocument reference= new BsonDocument
            {
                {"Id", "100" }
            };
            Simple source = new Simple();
            source.Id = 100;

            var result = RestaurantsController.CreateBson(source);
            result.ToString().Should().Be(reference.ToString());

        }
        private class Simple
        {
            public int Id { get; set; }
        }
    }
}