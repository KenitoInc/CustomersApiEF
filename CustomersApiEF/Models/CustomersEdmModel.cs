using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;

namespace CustomersApiEF.Models
{
    public static class CustomersEdmModel
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            builder.EntitySet<Order>("Orders");

            return builder.GetEdmModel();
        }
    }
}
