using System;
using System.Collections.Generic;
using System.Text;

namespace BryceFamily.Repo.Core.Model
{
    public class Descendant
    {
        public int Level { get; set; }
        public int PersonID { get; set; }
        public List<Descendant> Descendants { get; set; }

    }
}
