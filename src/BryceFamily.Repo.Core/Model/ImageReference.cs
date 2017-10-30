namespace BryceFamily.Repo.Core.Model
{
    public class ImageReference : Entity
    {

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageLocation { get; set; }

        public ImageReferenceType ImageReferenceType { get; set; }

        public string MimeType { get; set; }
    }
}
