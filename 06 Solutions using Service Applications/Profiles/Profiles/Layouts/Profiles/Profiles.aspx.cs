using System;
using System.Collections;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Profiles.Layouts.Profiles
{
    public partial class Profiles : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CreateProfile();

            ChangeProfile();
        }

        private static void ChangeProfile()
        {
// Replace "domain\\username" and "servername" with actual values.
            string targetAccountName = "spdom\\donald.duck";
            using (SPSite site = new SPSite("http://sp2016"))
            {
                SPServiceContext serviceContext = SPServiceContext.GetContext(site);
                try
                {
                    // Retrieve and iterate through all of the user profiles in this context.
                    Console.WriteLine("Retrieving user profiles:");
                    UserProfileManager userProfileMgr = new UserProfileManager(serviceContext);
                    IEnumerator userProfiles = userProfileMgr.GetEnumerator();
                    while (userProfiles.MoveNext())
                    {
                        UserProfile userProfile = (UserProfile) userProfiles.Current;
                        Console.WriteLine(userProfile.AccountName);
                    }

                    // Retrieve a specific user profile. Change the value of a user profile property
                    // and save (commit) the change on the server.
                    UserProfile user = userProfileMgr.GetUserProfile(targetAccountName);
                    Console.WriteLine("\\nRetrieving user profile for " + user.DisplayName + ".");
                    user.DisplayName = "The Don";
                    user.Commit();
                    Console.WriteLine("\\nThe user\\'s display name has been changed.");
                    Console.Read();
                }

                catch (System.Exception e)
                {
                    Console.WriteLine(e.GetType().ToString() + ": " + e.Message);
                    Console.Read();
                }
            }
        }

        private static void CreateProfile()
        {
            SPServiceContext serviceContext = SPServiceContext.GetContext("http://sp2016");
            try
            {
                // Create a user profile that uses the default user profile
                // subtype.
                UserProfileManager userProfileMgr = new UserProfileManager(serviceContext);
                UserProfile userProfile = userProfileMgr.CreateUserProfile(@"spdom\donald.duck");

                Console.WriteLine("A profile was created for " + userProfile.DisplayName);
                Console.Read();
            }

            catch (System.Exception ex)
            {
                Console.WriteLine(ex.GetType().ToString() + ": " + ex.Message);
                Console.Read();
            }
        }
    }
}
