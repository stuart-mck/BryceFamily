using System;
using BryceFamily.Repo.Core.Model;

namespace BryceFamily.Web.MVC.Models
{
    public class ImageReferenceModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Reference { get; set; }

        public string Description { get; set; }

        public string MimeType { get; set; }

        public ImageReference MapToEntity()
        {
            return new ImageReference()
            {
                Title = Title,
                ImageLocation = Reference,
                ID =  Id,
                Description = Description,
                MimeType = MimeType
            };
        }

        internal static ImageReferenceModel Map(Repo.Core.Model.ImageReference arg)
        {
            if (arg == null)
                return null;

            return new ImageReferenceModel()
            {
                MimeType = arg.MimeType,
                Reference = arg.ImageLocation,
                Title = arg.Title,
                Id = arg.ID
            };
        }
    }
}
