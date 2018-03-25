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
            string personListName = "Person";
            string[] jobs = { "Developer", "Admin", "Organization" };
            string[] skills = {"C#", "TypeScript", "React"};

            SPSite col = new SPSite("http://SP2016");
            SPWeb web = col.RootWeb;
            
            SPList list = web.Lists.TryGetList(personListName);

            if (list == null)
            {
                var skillslist = addListWithItems(web, "Skills", "A skills List", skills);               
                var joblist = addListWithItems(web, "Jobs", "A job List", jobs);

                Guid id = web.Lists.Add(personListName, "A Person List", SPListTemplateType.GenericList);
                list = web.Lists.TryGetList(personListName);

                var jobCol = list.Fields.AddLookup("Jobs", joblist.ID, true);
                SPFieldLookup jobLookup = (SPFieldLookup)list.Fields[jobCol];
                jobLookup.LookupField = joblist.Fields["Title"].InternalName;
                jobLookup.Title = "Job";                
                jobLookup.Update();
                list.Update();

                var skillCol = list.Fields.AddLookup("Skills", skillslist.ID, true);
                SPFieldLookup skillLookup = (SPFieldLookup)list.Fields[skillCol];
                skillLookup.LookupField = skillslist.Fields["Title"].InternalName;
                skillLookup.Title = "Skills";
                skillLookup.AllowMultipleValues = true;
                skillLookup.Update();
                list.Update();

                var person = list.Items.Add();
                person["Title"] = "Bob der Baumeister";
                person["Job"] = new SPFieldLookupValue(1, jobs[0]);
                var sks = new SPFieldLookupValueCollection
                {
                    new SPFieldLookupValue(1, skills[0]),
                    new SPFieldLookupValue(3, skills[2])
                };
                person["Skills"] = sks;
                person.Update();
            }
            
            foreach (SPListItem item in list.Items)
            {
                SPFieldLookupValue job = new SPFieldLookupValue(item["Job"].ToString());
                string skillsString = GetSkills(item);
                Console.WriteLine("{0} works as a {1} with skills {2}",item["Title"],job.LookupValue, skillsString); 
            }
        }

        private static SPList addListWithItems(SPWeb web, string ListName, string Descr, string [] items)
        {
            Guid id = web.Lists.Add(ListName, Descr, SPListTemplateType.GenericList);
            SPList list = web.Lists[id];
            foreach (var jn in items)
            {
                SPListItem li = list.Items.Add();
                li["Title"] = jn;
                li.Update();
            }
            return list;
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
