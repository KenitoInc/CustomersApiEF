using CustomersApiEF.Models;
using Microsoft.AspNet.OData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomersApiEF.Handlers
{
    public class OrdersHandler : ODataAPIHandler<Order>
    {
        Customer parent;

        public OrdersHandler(Customer parent)
        {
            this.parent = parent;
        }
        public override IODataAPIHandler GetNestedHandler(Order parent, string navigationPropertyName)
        {
            throw new NotImplementedException();
        }

        public override ODataAPIResponseStatus TryCreate(IDictionary<string, object> keyValues, out Order createdObject, out string errorMessage)
        {
            createdObject = null;
            errorMessage = string.Empty;

            try
            {
                createdObject = new Order();
                parent.Orders.Add(createdObject);

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
                //throw new NotImplementedException();

                // Comment the code below and uncomment the line above to test failed deletes.
                var id = keyValues.First().Value.ToString();
                var order = parent.Orders.First(x => x.Id == Int32.Parse(id));

                parent.Orders.Remove(order);

                return ODataAPIResponseStatus.Success;
                // --End of code to comment
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

                return ODataAPIResponseStatus.Failure;
            }
        }

        public override ODataAPIResponseStatus TryGet(IDictionary<string, object> keyValues, out Order originalObject, out string errorMessage)
        {
            ODataAPIResponseStatus status = ODataAPIResponseStatus.Success;
            errorMessage = string.Empty;
            originalObject = null;

            try
            {
                var id = keyValues["Id"].ToString();
                originalObject = parent.Orders.FirstOrDefault(x => x.Id == Int32.Parse(id));

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

        public override ODataAPIResponseStatus TryAddRelatedObject(Order resource, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                parent.Orders.Add(resource);

                return ODataAPIResponseStatus.Success;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

                return ODataAPIResponseStatus.Failure;
            }
        }
    }
}
