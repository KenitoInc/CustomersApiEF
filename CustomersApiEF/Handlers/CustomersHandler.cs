using CustomersApiEF.Models;
using Microsoft.AspNet.OData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomersApiEF.Handlers
{
    public class CustomersHandler : ODataAPIHandler<Customer>
    {
        CustomersDbContext db = null;

        public CustomersHandler(CustomersDbContext dbContext)
        {
            this.db = dbContext;
        }
        public override IODataAPIHandler GetNestedHandler(Customer parent, string navigationPropertyName)
        {
            switch (navigationPropertyName)
            {
                case "Orders":
                    return new OrdersHandler(parent);
                default:
                    return null;
            }
        }

        public override ODataAPIResponseStatus TryCreate(IDictionary<string, object> keyValues, out Customer createdObject, out string errorMessage)
        {
            createdObject = null;
            errorMessage = string.Empty;

            try
            {
                createdObject = new Customer();
                db.Customers.Add(createdObject);

                return ODataAPIResponseStatus.Success;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

                return ODataAPIResponseStatus.Failure;
            }
        }

        public override ODataAPIResponseStatus TryDelete(IDictionary<string, object> keyValues, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                var id = keyValues.First().Value.ToString();
                var customer = db.Customers.First(x => x.Id == Int32.Parse(id));

                db.Customers.Remove(customer);

                return ODataAPIResponseStatus.Success;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

                return ODataAPIResponseStatus.Failure;
            }
        }

        public override ODataAPIResponseStatus TryGet(IDictionary<string, object> keyValues, out Customer originalObject, out string errorMessage)
        {
            ODataAPIResponseStatus status = ODataAPIResponseStatus.Success;
            errorMessage = string.Empty;
            originalObject = null;

            try
            {
                var id = keyValues["Id"].ToString();
                originalObject = db.Customers.FirstOrDefault(x => x.Id == Int32.Parse(id));

                if (originalObject == null)
                {
                    status = ODataAPIResponseStatus.NotFound;
                }
            }
            catch (Exception ex)
            {
                status = ODataAPIResponseStatus.Failure;
                errorMessage = ex.Message;
            }

            return status;
        }

        public override ODataAPIResponseStatus TryAddRelatedObject(Customer resource, out string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
