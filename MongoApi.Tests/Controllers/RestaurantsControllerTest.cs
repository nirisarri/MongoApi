
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
        }
        private class Simple
        {
            public int Id { get; set; }
        }
    }
}