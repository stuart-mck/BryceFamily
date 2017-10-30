using System.Collections.Generic;

namespace BryceFamily.Web.MVC.Infrastructure
{
    public class ClanService
    {

        private readonly IReadOnlyList<string> _clans = new List<string>()
        {
            "Bryce",
            "McKenzie",
            "Harman"
        };

        public IReadOnlyList<string> Clans => _clans;
    }
}
