using Amazon.DynamoDBv2.DataModel;
using System;

namespace BryceFamily.Repo.Core.Model
{
    
    public class Entity<TId>
    {

        [DynamoDBHashKey]   
        public TId ID { get; set; }

        public DateTime LastUpdated { get; set; }

    }
}
