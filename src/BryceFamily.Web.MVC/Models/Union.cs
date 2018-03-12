using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BryceFamily.Web.MVC.Models
{
    public class Union
    {
        public Guid Id { get; set; }
        public Person Partner1 { get; set; }
        public Person Partner2 { get; set; }
        [DisplayName("Date of Union")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DateOfUnion { get; set; }
        [DisplayName("Date of Dissolution")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DateOfDissolution{ get; set; }
        public List<Person> Descendents { get; set; }

        public bool Divorced { get; set; }

        public int YearsMarried
        {
            get
            {
                if (!DateOfUnion.HasValue)
                    return -1;
                return Convert.ToInt32(Math.Floor((((DateTime.Now - DateOfUnion.Value).TotalDays) / 365)));
            }
        }

        internal Repo.Core.Model.Union Map()
        {
            return new Repo.Core.Model.Union()
            {
                DivorceDate = this.DateOfDissolution,
                ID = this.Id,
                IsEnded = this.Divorced,
                MarriageDate = this.DateOfUnion,
                Partner2ID = this.Partner2?.Id,
                PartnerID = this.Partner1?.Id
            };
        }


    }
}
