# CustomersApiEF
Sample project to demonstrate how we use ODataAPIHandler in OData bulk operations

## What is a Bulk Operation?  

A bulk operation is an operation that allows you to carry out several operations (insert, update, delete) on several/single resources (of the same type) and their nested resources (to whichever level) using a single request.

In this sample project we have Customer entity with related Order. The relationship is many to many.

### Model classes
```csharp
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
```

### Bulk operations request

```json
PATCH http://localhost:6285/odata/Customers
Host: host 
Content-Type: application/json
Prefer: odata.include-annotations="*"

{
    "@odata.context":"http://localhost:6285/odata/$metadata#Customers/$delta",
    "value":[
        {
            "@odata.type":"CustomersApiEF.Models.Customer",
            "Id":1,
            "Name":"Customer1",
            "Orders@odata.delta":[
                {
                    "@odata.id":"Customers(3)/Orders(1005)",
                    "Quantity": 1005
                },
                {
                    "Id": 2000,
                    "Price":200,
                    "Quantity":90
                }
            ]
        },
        {
            "@odata.type":"CustomersApiEF.Models.Customer",
            "Id":2,
            "Name":"Customer2",
            "Orders@odata.delta":[
                {
                    "@odata.removed" : {"reason":"changed"},
                    "Id":10
                },
                {
                    "@odata.removed" : {"reason":"changed"},
                    "Id":1004
                },
                {
                    "Id":12,
                    "Price":12,
                    "Quantity":12
                },
                {
                    "Id":13,
                    "Price":13,
                    "Quantity":13
                },
                {
                    "Id":1003,
                    "Price":999,
                    "Quantity":99
                }
            ]
        }
    ]
} 
```

The response will be as follows:

```json
{
    "@context": "http://localhost:6285/odata/$metadata#Customers/$delta",
    "value": [
        {
            "Id": 1,
            "Name": "Customer1",
            "Age": 0,
            "Orders@delta": [
                {
                    "Id": 1005,
                    "Price": 40,
                    "Quantity": 1005
                },
                {
                    "Id": 2000,
                    "Price": 200,
                    "Quantity": 90
                }
            ]
        },
        {
            "Id": 2,
            "Name": "Customer2",
            "Age": 0,
            "Orders@delta": [
                {
                    "@removed": {
                        "reason": "changed"
                    },
                    "@id": "http://localhost:6285/odata/Orders(10)",
                    "@Core.DataModificationException": {
                        "@type": "#Org.OData.Core.V1.DataModificationExceptionType"
                    },
                    "Id": 10,
                    "Price": 0,
                    "Quantity": 0
                },
                {
                    "@removed": {
                        "reason": "changed"
                    },
                    "@id": "http://localhost:6285/odata/Orders(1004)",
                    "Id": 1004,
                    "Price": 0,
                    "Quantity": 0
                },
                {
                    "Id": 12,
                    "Price": 12,
                    "Quantity": 12
                },
                {
                    "Id": 13,
                    "Price": 13,
                    "Quantity": 13
                },
                {
                    "Id": 1003,
                    "Price": 999,
                    "Quantity": 99
                }
            ]
        }
    ]
}
```

