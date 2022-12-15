using CustomersApiEF.Helpers;
using CustomersApiEF.Models;
using Microsoft.AspNet.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using System.Collections.Generic;
using System.Linq;

namespace CustomersApiEF.Handlers
{
    public class ApiHandlerFactory : ODataAPIHandlerFactory
    {
        CustomersDbContext db = null;
        public ApiHandlerFactory(IEdmModel model, CustomersDbContext dbContext) : base(model)
        {
            this.db = dbContext;
        }
        public override IODataAPIHandler GetHandler(ODataPath odataPath)
        {
            if (odataPath != null)
            {
                int currentPosition = 0;

                if (odataPath.Count == 1)
                {
                    GetHandlerInternal(odataPath.FirstSegment.Identifier, currentPosition);
                }

                List<ODataPathSegment> pathSegments = odataPath.ToList();

                ODataPathSegment currentPathSegment = pathSegments[currentPosition];

                if (currentPathSegment is EntitySetSegment || currentPathSegment is NavigationPropertySegment || currentPathSegment is SingletonSegment)
                {
                    int keySegmentPosition = ODataPathHelper.GetNextKeySegmentPosition(pathSegments, currentPosition);
                    KeySegment keySegment = (KeySegment)pathSegments[keySegmentPosition];

                    currentPosition = keySegmentPosition;

                    return GetHandlerInternal(
                        currentPathSegment.Identifier,
                        currentPosition,
                        ODataPathHelper.KeySegmentAsDictionary(keySegment),
                        pathSegments);
                }
            }

            return null;
        }

        private IODataAPIHandler GetHandlerInternal(
            string pathName,
            int currentPosition,
            Dictionary<string, object> keys = null,
            List<ODataPathSegment> pathSegments = null)
        {
            switch (pathName)
            {
                case "Customers":
                    Customer customer;
                    string msg;
                    if ((new CustomersHandler(db).TryGet(keys, out customer, out msg)) == ODataAPIResponseStatus.Success)
                    {
                        return GetNestedHandlerForCustomer(pathSegments, currentPosition, customer);
                    }
                    return null;
                default:
                    return null;
            }
        }

        private IODataAPIHandler GetNestedHandlerForCustomer(List<ODataPathSegment> pathSegments, int currentPosition, Customer customer)
        {
            ++currentPosition;

            if (pathSegments.Count <= currentPosition)
            {
                return null;
            }

            ODataPathSegment currentPathSegment = pathSegments[currentPosition];

            if (currentPathSegment is NavigationPropertySegment)
            {
                switch (currentPathSegment.Identifier)
                {
                    case "Orders":
                        return new OrdersHandler(customer);

                    default:
                        return null;
                }
            }
            return null;
        }
    }
}
