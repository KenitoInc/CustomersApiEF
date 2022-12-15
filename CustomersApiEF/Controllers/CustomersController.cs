using CustomersApiEF.Handlers;
using CustomersApiEF.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CustomersApiEF.Controllers
{
    public class CustomersController : ODataController
    {
        CustomersDbContext db;
        public CustomersController(CustomersDbContext db)
        {
            this.db = db;
            DataSeeder.SeedDatabase(db);
        }

        [EnableQuery]
        public IQueryable<Customer> Get()
        {
            return db.Customers.AsQueryable<Customer>();
        }

        [EnableQuery]
        public Customer Get(int key)
        {
            return db.Customers.Where(b => b.Id == key).Single();
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            try
            {
                db.Customers.Add(customer);
                await db.SaveChangesAsync();
                return Created(customer);
            }
            catch
            {
                db.Customers.Local.Remove(customer);
                return BadRequest();
            }
        }

        [EnableQuery]
        public IActionResult Patch([FromODataUri] int key, Delta<Customer> delta)
        {
            var customer = db.Customers.FirstOrDefault(b => b.Id == key);
            if (customer == null)
            {
                return NotFound();
            }
            delta.Patch(customer);
            db.SaveChanges();
            return Updated(customer);
        }

        [EnableQuery]
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Customer customer)
        {
            if (key != customer.Id)
            {
                return BadRequest();
            }
            db.Entry(customer).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Updated(customer);
        }

        [EnableQuery]
        public async Task<IActionResult> Delete(int key)
        {
            var customer = db.Customers.FirstOrDefault(b => b.Id == key);
            if (customer == null)
            {
                return NotFound();
            }
            db.Customers.Remove(customer);
            await db.SaveChangesAsync();
            return Ok(customer);
        }

        [ODataRoute("Customers")]
        [HttpPatch]
        [EnableQuery]
        public IActionResult PatchCustomers([FromBody] DeltaSet<Customer> coll)
        {
            var returncoll = coll.Patch(new CustomersHandler(db), new ApiHandlerFactory(Request.GetModel(),db));

            db.SaveChanges();

            return Ok(returncoll);
        }

        [ODataRoute("Customers({key})/Orders")]
        [HttpPatch]
        [EnableQuery]
        public IActionResult PatchCustomerOrders(int key, [FromBody] DeltaSet<Order> coll)
        {
            var customer = db.Customers.First(x => x.Id == key);

            var returncoll = coll.Patch(new OrdersHandler(customer), new ApiHandlerFactory(Request.GetModel(), db));

            db.SaveChanges();

            return Ok(returncoll);
        }
    }
}
