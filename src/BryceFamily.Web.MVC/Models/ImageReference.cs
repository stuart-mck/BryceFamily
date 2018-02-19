﻿using System;
using BryceFamily.Repo.Core.Model;

namespace BryceFamily.Web.MVC.Models
{
    public class ImageReferenceModel
    {
        public Guid Id { get; set; }

        public Guid GalleryReference { get; set; }

        public string Title { get; set; }

        public string Reference { get; set; }

        public string Description { get; set; }

        public string MimeType { get; set; }

        public bool DefaultGalleryImage { get; set; }

        public int PersonId { get; set; }

     
        public string ThumbnailSizeLink => $"{Reference}/thumbnail/{Title}";

        public ImageReference MapToEntity()
        {
            return new ImageReference()
            {
                Title = Title,
                ImageLocation = Reference,
                ID = GalleryReference,
                Description = Description,
                MimeType = MimeType,
                ImageID = Id,
                DefaultGalleryImage = DefaultGalleryImage,
                GalleryId = GalleryReference,
                PersonId = PersonId
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
                Id = arg.ImageID,
                GalleryReference = arg.ID,
                DefaultGalleryImage =  arg.DefaultGalleryImage
            };
        }
    }
}
