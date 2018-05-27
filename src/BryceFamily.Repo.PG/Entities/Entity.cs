using System;

namespace BryceFamily.Repo.Core.Model
{
    
    public class Entity<TId>
    {
        
        public TId ID { get; set; }

        public DateTime LastUpdated { get; set; }

        public DateTime DateCreated { get; set; }



    }
}
