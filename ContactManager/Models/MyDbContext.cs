using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ContactManager.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) {
            Database.EnsureCreated();
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
