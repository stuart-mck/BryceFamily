using System;
using System.Collections.Generic;
using System.Text;

namespace BryceFamily.Repo.Core.Model
{
    public class SpousalRelationship 
    {
        public int HusbandID { get; set; }

        public int WifeID { get; set; }

        public DateTime? MarriageDate { get; set; }

        public DateTime? DivorceDate { get; set; }

    }
}
