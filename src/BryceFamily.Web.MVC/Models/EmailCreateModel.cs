using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Models
{
    public class EmailCreateModel
    {

        public string Subject { get; set; }

        public string From { get; set; }

        public string MessageContent { get; set; }
    }
}
