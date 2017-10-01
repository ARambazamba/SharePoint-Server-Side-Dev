using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;

namespace PermissionConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // create site, list, set security
            SPSite col = new SPSite("http://sp2016");

            SPWeb root = col.RootWeb;

            foreach (SPWebTemplate t in root.GetAvailableWebTemplates(1033))
            {
                Console.WriteLine(t.Name + " " + t.Title);
            }

            SPWeb web = root.Webs.Add("CodedWeb", "CodedWeb", "Demo Description", 1033, "STS#1", true, false);

            // add navigation
            SPNavigation navRoot = root.Navigation;
            SPNavigationNodeCollection navQL = navRoot.QuickLaunch;

            var newNav = new SPNavigationNode(web.Title, web.ServerRelativeUrl);
            var ms = new SPNavigationNode("Microsoft", "http://www.microsoft.com");

            navQL.Add(newNav, newNav);

            // alternative
            // web.QuickLaunchEnabled = true;

            Guid listid = web.Lists.Add("My Pictures", "PersonalPictures", SPListTemplateType.PictureLibrary);
            SPList list = web.Lists[listid];
            list.OnQuickLaunch = true;

            // list permission levels
            foreach (SPRoleDefinition PermLevel in list.ParentWeb.RoleDefinitions)
            {
                Console.WriteLine(string.Format("Role: {0}, ID: {1}", PermLevel.Name, PermLevel.Id));
            }

            // list groups
            foreach (SPGroup gp in list.ParentWeb.SiteGroups)
            {
                Console.WriteLine(string.Format("Group: {0}", gp.Name));
            }

            // break rights inheritance - boolean: copy permissions
            list.BreakRoleInheritance(false);

            // assign permissions
            list.ParentWeb.SiteUsers.Add(@"spdom\hsimpson", "homer.simpson@spdom.local", "Homer Simpson", "A yellow comic guy");

            SPUser usrHomer = list.ParentWeb.SiteUsers[@"spdom\hsimpson"]; //login name

            if (usrHomer != null)
            {
                // lookup permission level full control
                SPRoleDefinition PermLevelFull = list.ParentWeb.RoleDefinitions["Full Control"];

                SPRoleAssignment assignment = new SPRoleAssignment(usrHomer);

                // Bind Assignment to definition
                assignment.RoleDefinitionBindings.Add(PermLevelFull);

                list.RoleAssignments.Add(assignment);

                list.Update();

                // Auslesen von Permissions
                foreach (SPRoleAssignment rs in list.RoleAssignments)
                {
                    //List Security Principals
                    Console.WriteLine(rs.Member.Name);

                    foreach (SPRoleDefinition rd in rs.RoleDefinitionBindings)
                    {
                        //Rechte 
                        Console.WriteLine(rd.Name);
                    }
                }

                //Same Pattern for Groups
                SPGroup mgr = list.ParentWeb.Groups["Manager"];
                if (mgr != null)
                {
                    SPRoleAssignment ass = new SPRoleAssignment(mgr);
                    // ....
                }
            }
        }
    }
}
