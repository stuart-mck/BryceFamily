using System.Collections.Generic;

namespace BryceFamily.Repo.Core.Model
{
    public class Image : Entity
    {
        public byte[] ImageData { get; set; }

        public ICollection<ImageTag> Tags { get; set; }
    }
}
