﻿using A2.Models;
using Microsoft.EntityFrameworkCore;

namespace A2.Data
{
    public class A2DbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<Event> Events { get; set; }

        public A2DbContext(DbContextOptions<A2DbContext> options)
            : base(options)
        {
        }
    }
}