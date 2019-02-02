using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Models.Image
{
    public class ImageViewModel
    {
        public Guid GalleryId { get; set; }
        public Guid ImageId { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }

        public string FullSizeLink
        {
            get
            {
                return $"/{GalleryId}/{ImageId}{Path.GetExtension(FileName)}";
            }
        }

        public string Description { get; set; }

        public Guid PreviousLink { get; set; }

        public Guid NextLink { get; set; }

    }
}
