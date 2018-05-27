using BryceFamily.Repo.PG.Entities;
using Dapper;
using System.Linq;

namespace BryceFamily.Repo.PG.Repository
{
    public class PersonRepository
    {
        private readonly ConnectionResolver _resolver;

        public PersonRepository(ConnectionResolver resolver)
        {
            _resolver = resolver;
        }

        public Person Load(int id)
        {
            using (var connection = _resolver.GetConnection())
            {
                return connection.Query<Person>("Select * From person " +
                                                "WHERE id = @Id", new { id }).SingleOrDefault();
            }
        }
    }
}
