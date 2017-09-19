using System;

namespace BryceFamily.Web.MVC.Models
{
    public class ImageReferenceModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public Guid Reference { get; set; }

        public string MimeType { get; set; }

        internal static ImageReferenceModel Map(Repo.Core.Model.ImageReference arg)
        {
            if (arg == null)
                return null;

            return new ImageReferenceModel()
            {
                MimeType = arg.MimeType,
                Reference = arg.ID,
                Title = arg.Title
            };
        }
    }
}
