using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECT.CustomersModel
{
    /// <summary>
    /// All the methods for retrieving, updating and deleting data are implemented in this class file.
    /// The samples below show the finder and specific finder method for Entity1.
    /// </summary>
    public class CustomerService
    {
        public static Dictionary<Int32, Customer> d = null;
        public static Customer ReadItem(Int32 id)
        {
            // take a copy for SharePoint
            Customer c = new Customer();
            Customer e = d[id];
            c.CustomerID = e.CustomerID;
            c.FirstName = e.FirstName;
            c.LastName = e.LastName;
            c.Message = e.Message;
            return c;
        }
        public static IEnumerable<Customer> ReadList()
        {
            // this is usually the first method called so check for null
            if (d == null)
            {
                d = new Dictionary<Int32, Customer>();
                for (int i = 0; i < 10; i++)
                {
                    Customer e = new Customer
                        {
                            CustomerID = i,
                            Message = i + " Item Data",
                            FirstName = i + " First Name",
                            LastName = i + " Last Name"
                        };
                    d.Add(i, e);
                }
            }
            return d.Values;
        }

        public static void Update(Customer customer, Int32 id)
        {
            d[id].FirstName = customer.FirstName;
            d[id].LastName = customer.LastName;
            d[id].Message = customer.Message;
        }
    }
}
