using Microsoft.AspNet.OData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomersApiEF.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public int Age { get; set; }
        public List<Order> Orders { get; set; }
    }

    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public List<Customer> Customers { get; set; }
        public ODataIdContainer Container { get; set; }
    }
}
