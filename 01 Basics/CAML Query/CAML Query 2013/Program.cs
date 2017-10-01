using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.SharePoint;

namespace CAML_Query_2013
{
    class Program
    {
        static void Main(string[] args)
        {

            var col = new SPSite("http://sp2016");
            SPWeb web = col.RootWeb;
            SPList list = web.Lists["Products"];

            SPQuery qry = new SPQuery
            {
                Query = @"<Where><Lt><FieldRef Name='Price' /><Value Type='Number'>600</Value></Lt></Where>",
                ViewFields = @"<FieldRef Name='Title' /><FieldRef Name='Price' />"
            };
            SPListItemCollection filteredItems = list.GetItems(qry);

            Debug.WriteLine(string.Format("Found {0} items in query.", filteredItems.Count));
            foreach (SPListItem item in filteredItems)
            {
                Debug.WriteLine(string.Format("Found item: {0} modiefied on {1}",
                                              item["Title"],
                                              item["Modified"]));
            }
        }
    }
}
