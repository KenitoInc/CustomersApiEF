using System.Collections.Generic;

namespace CustomersApiEF.Models
{
    public class DataSeeder
    {
        public static void SeedDatabase(CustomersDbContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var c1 = new Customer { Id = 1, Name = "Customer 1", Age = 31 };
            var c2 = new Customer { Id = 2, Name = "Customer 2", Age = 32 };
            var c3 = new Customer { Id = 3, Name = "Customer 3", Age = 33 };
            var c4 = new Customer { Id = 4, Name = "Customer 4", Age = 34 };
            var c5 = new Customer { Id = 5, Name = "Customer 5", Age = 35 };

            db.Customers.Add(c1);
            db.Customers.Add(c2);
            db.Customers.Add(c3);
            db.Customers.Add(c4);
            db.Customers.Add(c5);

            var o1 = new Order { Id = 1001, Price = 10, Quantity = 10 };
            var o2 = new Order { Id = 1002, Price = 35, Quantity = 2 };
            var o3 = new Order { Id = 1003, Price = 70, Quantity = 5 };
            var o4 = new Order { Id = 1004, Price = 20, Quantity = 20 };
            var o5 = new Order { Id = 1005, Price = 40, Quantity = 15 };
            var o6 = new Order { Id = 1006, Price = 15, Quantity = 50 };
            var o7 = new Order { Id = 1007, Price = 60, Quantity = 25 };
            var o8 = new Order { Id = 1008, Price = 55, Quantity = 30 };
            var o9 = new Order { Id = 1009, Price = 80, Quantity = 10 };
            var o10 = new Order { Id = 1010, Price = 50, Quantity = 20 };

            db.Orders.Add(o1);
            db.Orders.Add(o2);
            db.Orders.Add(o3);
            db.Orders.Add(o4);
            db.Orders.Add(o5);
            db.Orders.Add(o6);
            db.Orders.Add(o7);
            db.Orders.Add(o8);
            db.Orders.Add(o9);
            db.Orders.Add(o10);

            db.SaveChanges();

            c1.Orders = new List<Order>() { o1, o2 };
            c2.Orders = new List<Order>() { o3, o4 };
            c3.Orders = new List<Order>() { o5, o6 };
            c4.Orders = new List<Order>() { o7, o8 };
            c5.Orders = new List<Order>() { o9, o10 };

            db.SaveChanges();
        }
    }
}
