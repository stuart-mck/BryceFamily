using System;
using System.Collections.Generic;
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
                var fileName = string.IsNullOrEmpty(FileName)
                                ? Title : FileName;
                return $"familybryce.gallery/{GalleryId}/{fileName}";
            }
        }

        public string Description { get; set; }

        public Guid PreviousLink { get; set; }

        public Guid NextLink { get; set; }

    }
}
