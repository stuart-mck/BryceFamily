using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;

namespace BryceFamily.Repo.Core.Model
{
    [DynamoDBTable("gallery")]
    public class Gallery : Entity
    {
        public string Name { get; set; }

        public Guid Owner { get; set; }

        public string Summary { get; set; }

        public Guid FamilyEvent { get; set; }

        public string Family { get; set; }

        public DateTime DateCreated { get; set; }


    }
}
