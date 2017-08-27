using brycefamily.repo.Model;
using Microsoft.EntityFrameworkCore;

namespace brycefamily.repo
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }
    }
}
