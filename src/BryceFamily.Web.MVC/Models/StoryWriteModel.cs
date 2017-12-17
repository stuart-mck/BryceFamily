using System;

namespace BryceFamily.Web.MVC.Models
{
    public class StoryWriteModel
    {
        public int PersonID { get; set; }

        public Guid StoryID { get; set; }

        public string Title { get; set; }

        public string Story { get; set; }
    }
}
