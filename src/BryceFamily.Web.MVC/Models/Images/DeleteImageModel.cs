using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Models.Images
{
    public class DeleteImageModel
    {
        public Guid Id { get; set; }

        public Guid GalleryReference { get; set; }

    }
}
