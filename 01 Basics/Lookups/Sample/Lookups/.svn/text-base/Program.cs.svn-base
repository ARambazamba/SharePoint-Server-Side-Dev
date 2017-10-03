using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;

namespace Lookups
{
    class Program
    {
        static void Main(string[] args)
        {

            SPSite col = new SPSite("http://chiron");
            SPWeb web = col.AllWebs["Lookups"];
            SPList list = web.Lists["Person"];

            foreach (SPListItem item in list.Items)
            {
                SPFieldLookupValue job = new SPFieldLookupValue(item["Job"].ToString());
                string skills = GetSkills(item);
                Console.WriteLine("{0} works as a {1} with skills {2}",item["Title"],job.LookupValue,skills); 
            }
        }

        static string GetSkills(SPListItem Item)
        {
            string result = string.Empty;

            SPFieldLookupValueCollection skills = new SPFieldLookupValueCollection(Item["Skills"].ToString());
            for (int i = 0; i < skills.Count; i++)
            {
                SPFieldLookupValue skill = skills[i];
                result += skill.LookupValue + ";";
            }
            return result;
        }
    }
}
