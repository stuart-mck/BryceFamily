using System;
using System.Collections.Generic;
using System.Text;

namespace BryceFamily.Repo.Core.Write.Query
{
    public class StoryQuery : IQueryParameter
    {
        public Guid StoryId { get; set; }

    }
}
