using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BryceFamily.Repo.PG.Config
{
    public class DataEnvironment
    {
        private readonly string _environment;
        private readonly List<string> _validEnvironments = new List<string> {"local", "production" };

        public DataEnvironment(string environment)
        {
            if (!_validEnvironments.Any(env => env == environment))
                throw new ArgumentException($"Unsupported Environment {environment}");

            _environment = environment;
        }

        public string Value
        {
            get
            {
                return _environment;
            }
        }


    }
}
