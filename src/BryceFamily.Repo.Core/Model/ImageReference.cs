using System;
using System.Collections.Generic;
using System.Text;

namespace BryceFamily.Repo.Core.Model
{
    public class ImageReference : Entity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Guid ImageLocation { get; set; }

        public string MimeType { get; set; }
    }
}
