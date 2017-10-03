using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECT.CustomersModel
{
    /// <summary>
    /// This class contains the properties for Entity1. The properties keep the data for Entity1.
    /// If you want to rename the class, don't forget to rename the entity in the model xml as well.
    /// </summary>
    public partial class Customer
    {
        public Int32 CustomerID { get; set; }
        public string Message { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
