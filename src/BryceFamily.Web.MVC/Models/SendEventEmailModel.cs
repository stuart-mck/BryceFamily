using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Models
{
    public class SendEventEmailModel
    {
        public Guid EventId { get; set; }

        public string EventTitle { get; set; }

        public string Subject { get; set; }
        public string Notes { get; set; }

    }
}
