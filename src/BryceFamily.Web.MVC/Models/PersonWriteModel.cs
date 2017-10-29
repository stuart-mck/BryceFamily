using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Models
{
    public class PersonWriteModel
    {
        public PersonWriteModel()
        {
            Person = new Person();
        }
        public Person Person { get; set; }
    }
}
