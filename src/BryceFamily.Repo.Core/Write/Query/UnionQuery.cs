using System;
using System.Collections.Generic;
using System.Text;

namespace BryceFamily.Repo.Core.Write.Query
{
    public class UnionQuery : IQueryParameter
    {
        public int Partner1Id { get; set; }

        public int Partner2Id { get; set; }
    }
}
