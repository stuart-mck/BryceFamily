using Amazon.DynamoDBv2.DataModel;
using System;

namespace BryceFamily.Repo.Core.Model
{
    
    public class Entity
    {

        [DynamoDBHashKey, DynamoDBProperty("id")]   
        public Guid ID { get; set; }

    }
}
