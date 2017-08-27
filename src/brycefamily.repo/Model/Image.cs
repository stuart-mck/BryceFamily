using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace brycefamily.repo.Model
{
    public class Image : Entity
    {
        public byte[] ImageData { get; set; }

        public string[] Tags { get; set; }
    }
}
