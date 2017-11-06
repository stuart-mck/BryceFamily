using System;

namespace BryceFamily.Repo.Core.Model
{
    public class ImageAssociation
    {
        public Guid ReferenceId { get; set; }
        public ImageReferenceType ImageReferenceType { get; set; }
    }
}
