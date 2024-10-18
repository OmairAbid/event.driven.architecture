using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace QueryRepository.Repository
{
    public abstract class QueryEntity : IQueryEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public long Version { get; set; }
    }
}
