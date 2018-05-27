using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BryceFamily.Repo.PG.Entities
{
    [Table("person")]
    public class Person
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("middle_name")]
        public string MiddleName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("mainden_name")]
        public string MaidenName { get; set; }

        [Column("name")]
        public string Phone { get; set; }

        [Column("address_1")]
        public string Address1 { get; set; }

        [Column("address_2")]
        public string Address2 { get; set; }

        [Column("suburb")]
        public string Suburb { get; set; }

        [Column("state")]
        public string State { get; set; }

        [Column("mother_id")]
        public int? MotherID { get; set; }

        [Column("father_id")]
        public int? FatherID { get; set; }

        [Column("parents_id")]

        public Guid ParentRelationship { get; set; }

        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        [Column("year_of_birth")]
        public int? YearOfBirth { get; set; }

        [Column("date_of_death")]
        public DateTime? DateOfDeath { get; set; }

        [Column("year_of_death")]
        public int? YearOfDeath { get; set; }

        [Column("post_code")]
        public string PostCode { get; set; }

        [Column("email_address")]
        public string EmailAddress { get; set; }

        [Column("email_address")]
        public bool SubscribeToEmail { get; set; }

        [Column("occupation")]
        public string Occupation { get; set; }

        [Column("gender")]
        public int Gender { get; set; }

        [Column("is_spouse")]
        public bool IsSpouse { get; set; }
    }
}
