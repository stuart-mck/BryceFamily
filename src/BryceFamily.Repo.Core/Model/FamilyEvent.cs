﻿using System;
using System.Collections.Generic;

namespace BryceFamily.Repo.Core.Model
{
    public class FamilyEvent : Entity
    {
        
        public string Title { get; set; }
        public DateTime StartDate { get; set; }

        public EventLocation Location { get; set; }

        public string Details { get; set; }
        public EventType EventType { get; set; }
        public EventStatus EventStatus { get; set; }
        public Person  PersonId { get; set; }
        public ICollection<Image> EventImages { get; set; }
    }
}