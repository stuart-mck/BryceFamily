using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BryceFamily.Repo.Core.Model
{
    public class RangeKeyEntity<TId, TRangeKey>
    {
        [DynamoDBHashKey]
        public TId ID { get; set; }

        [DynamoDBRangeKey]
        public TRangeKey RangeKey { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
