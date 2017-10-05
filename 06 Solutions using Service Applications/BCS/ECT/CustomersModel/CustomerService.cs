using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECT.CustomersModel
{
    public class CustomerService
    {
        public static Dictionary<Int32, Customer> customersDict = null;
        public static Customer ReadItem(Int32 id)
        {
            // take a copy for SharePoint
            Customer c = new Customer();
            Customer e = customersDict[id];
            c.CustomerID = e.CustomerID;
            c.FirstName = e.FirstName;
            c.LastName = e.LastName;
            c.Message = e.Message;
            return c;
        }
        public static IEnumerable<Customer> ReadList()
        {
            // this is usually the first method called so check for null
            if (customersDict == null)
            {
                customersDict = new Dictionary<Int32, Customer>();
                for (int i = 0; i < 10; i++)
                {
                    Customer e = new Customer
                        {
                            CustomerID = i,
                            Message = i + " Item Data",
                            FirstName = i + " First Name",
                            LastName = i + " Last Name"
                        };
                    customersDict.Add(i, e);
                }
            }
            return customersDict.Values;
        }

        public static void Update(Customer customer, Int32 id)
        {
            customersDict[id].FirstName = customer.FirstName;
            customersDict[id].LastName = customer.LastName;
            customersDict[id].Message = customer.Message;
        }
    }
}
