using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace brycefamily.repo.Model
{
    public class Person : Entity
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Image> Images { get; set; }

    }
}
