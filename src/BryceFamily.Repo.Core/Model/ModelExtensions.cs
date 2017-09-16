using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BryceFamily.Repo.Core.Model
{
    public static class ModelExtensions
    {
        public static FamilyEvent MapToFamilyEvent(this Document sourceDocument)
        {
            return new FamilyEvent();
        }
    }
}
