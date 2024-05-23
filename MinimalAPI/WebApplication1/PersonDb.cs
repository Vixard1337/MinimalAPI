using Microsoft.EntityFrameworkCore;
using MinimalAPI.Models;

namespace MinimalAPI
{
    public class PersonDb : DbContext
    {
        public PersonDb(DbContextOptions<PersonDb> options)
            : base(options) { }
        public DbSet<Person> Persons => Set<Person>();
    }
}
