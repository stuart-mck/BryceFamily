﻿using System;
using System.ComponentModel;

namespace BryceFamily.Web.MVC.Models
{
    public class NewUnionPersonModel
    {
        public NewUnionPersonModel()
        {

        }

        public NewUnionPersonModel(Person person)
        {
            PartnerId = person.Id;
            PartnerName = person.FullName;
            DateOfBirth = DateTime.Today;
            DateOfUnion = DateTime.Today;
        }

        public string PartnerName { get; }

        public int PartnerId { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Maiden Name")]
        public string MaidenName { get; set; }

        
        public string Gender { get; set; }

        [DisplayName("Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("Date Of Union")]
        public DateTime DateOfUnion { get; set; }

        public string  Occupation { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }


    }
}