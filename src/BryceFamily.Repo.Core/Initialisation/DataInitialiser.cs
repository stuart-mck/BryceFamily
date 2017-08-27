using System;
using System.Collections.Generic;
using System.Text;

namespace BryceFamily.Repo.Core.Initialisation
{
    public static class DataInitialiser
    {
        public static void Initialize(DatabaseContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
