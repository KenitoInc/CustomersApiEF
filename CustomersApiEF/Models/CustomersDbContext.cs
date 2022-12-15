using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomersApiEF.Models
{
    public class CustomersDbContext : DbContext
    {
        public CustomersDbContext(DbContextOptions<CustomersDbContext> options)
        : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var customer = modelBuilder.Entity<Customer>();
            customer.HasMany(c => c.Orders).WithMany(a => a.Customers);
            customer.HasKey(c => c.Id);

            var order = modelBuilder.Entity<Order>();
            order.Ignore(i => i.Container);
            order.Property(e => e.Id).ValueGeneratedNever();
        }
    }
}
