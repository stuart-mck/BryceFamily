using System;
using System.Collections.Generic;
using System.Text;

namespace BryceFamily.Repo.Core.Model
{
    public class Gallery : Entity
    {
        public string Name { get; set; }

        public Person Owner { get; set; }

        public string Summary { get; set; }

        public FamilyEvent FamilyEvent { get; set; }

        public List<ImageReference> ImageReferences { get; set; }

        public DateTime DateCreated { get; set; }


    }
}
