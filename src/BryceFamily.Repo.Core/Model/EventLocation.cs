using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Model
{
    public class EventLocation : Entity
    {
        public string Title { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Geo { get; set; }
        public string PhoneNumber { get; set; }
    }
}
