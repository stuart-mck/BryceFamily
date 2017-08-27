using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Model
{
    public class Image : Entity
    {
        public byte[] ImageData { get; set; }

        public string[] Tags { get; set; }
    }
}
