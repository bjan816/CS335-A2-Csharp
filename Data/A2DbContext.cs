using A2.Models;
using Microsoft.EntityFrameworkCore;

namespace A2.Data
{
    public class A2DbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public A2DbContext(DbContextOptions<A2DbContext> options)
            : base(options)
        {
        }
    }
}