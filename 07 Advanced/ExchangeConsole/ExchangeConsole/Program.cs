using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.SharePoint;
using ReiserIntranet;

namespace ExchangeConsole
{
    class Program
    {
        public static string WebURL = "http://sp2016/";
        public static string[] FldNames = { "DemoPublic"};
        public static string AdminUser = "Administrator";
        public static string AdminPWD = "Pa$$w0rd";
        public static string AdminDomain = "spdom.local";
        public static string ExchangeWebServiceURL = "https://outlook.office.com/ews/exchange.asmx";
        public static ExchangeVersion ExchangeVersion = ExchangeVersion.Exchange2013;
        
        private static void Main(string[] args)
        {
            ExchangeService service = new ExchangeService(ExchangeVersion)
            {
                Credentials = new WebCredentials(AdminUser, AdminPWD),
                Url = new Uri(ExchangeWebServiceURL)
            };

            List<Folder> flds = new List<Folder>();
            GetFolders(service, flds);
            List<ProfileInfo> contacts = new List<ProfileInfo>();
            foreach (Folder folder in flds)
            {
                Console.WriteLine("Processing folder " + folder.DisplayName);
                ItemView itemView = new ItemView(int.MaxValue);
                FindItemsResults<Item> searchResults = service.FindItems(folder.Id, itemView);

                foreach (var obj in searchResults)
                {
                    if (obj is Contact)
                    {
                        Contact item = (Contact)obj;
                        ProfileInfo p = new ProfileInfo
                        {
                            LastName = item.Surname,
                            FirstName = item.GivenName,
                            Folder = folder.DisplayName,
                            Nickname = item.NickName
                        };

                        if (item.PhoneNumbers != null && item.PhoneNumbers.Contains(PhoneNumberKey.BusinessPhone))
                        {
                            p.WorkPhone = item.PhoneNumbers[PhoneNumberKey.BusinessPhone];
                        }

                        if (item.JobTitle != null) p.Department = item.JobTitle;

                        if (item.CompanyName != null) p.Company = item.CompanyName;

                        try
                        {
                            if (item.EmailAddresses[0] != null)
                            {
                                string mail = item.EmailAddresses[0].Address;

                                if (mail.Contains("/"))
                                {
                                    var resolved = service.ResolveName(mail);
                                    if (resolved.Any() && resolved[0].Mailbox != null)
                                    {
                                        p.Email = resolved[0].Mailbox.Address;
                                    }
                                }
                                else
                                {
                                    p.Email = mail;
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        contacts.Add(p);
                    }
                }
            }

            SPSite site = new SPSite(WebURL);
            SPWeb web = site.RootWeb;

            var pPics = ImportProfilePictures(web);

            var lists = contacts.Select(f => f.Folder).Distinct().ToList();

            foreach (string l in lists)
            {
                SPList list = web.Lists.TryGetList(l);

                if (list != null)
                {
                    List<Guid> ids = (from SPListItem item in list.Items select item.UniqueId).ToList();

                    foreach (Guid id in ids)
                    {
                        list.GetItemByUniqueId(id).Delete();
                    }
                }
                else
                {
                    Guid id = web.Lists.Add(l, "A Reiser Contacts list", l.RemoveBlanks(), ReiserIntranetConfig.ReiserCoreFeature,
                    10005, string.Empty);
                    list = web.Lists[id];
                }

                var tc = contacts.Where(m => m.Folder == l);

                foreach (ProfileInfo item in tc)
                {
                    KeyValuePair<string, string> pic = pPics.FirstOrDefault(f => item.Nickname!=null && f.Key.ToLower() == item.Nickname.ToLower());

                    SPListItem li = list.Items.Add();
                    li["Title"] = item.LastName;
                    li["FirstName"] = item.FirstName;
                    li["EMail"] = item.Email;
                    li["Company"] = item.Company;
                    li["WorkPhone"] = item.WorkPhone;
                    li["Department"] = item.Department;
                    li["Nickname"] = item.Nickname;
                    li["ProfilePicture"] = pic.Key != null? pic.Value : string.Empty;
                    li.Update();
                    Console.WriteLine("Created Contact {0}", item.LastName);
                }
            }
            Console.WriteLine("Import Finished");
        }

        public static KeyValuePair<string, string> [] ImportProfilePictures(SPWeb web)
        {
            Console.WriteLine("Starting Profile Picture Import");
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            SPList profilePics = web.Lists.TryGetList(ReiserIntranetConfig.ProfilePictureList);
            DirectoryInfo profileFld = new DirectoryInfo(ReiserIntranetConfig.ProfileImportFolder);
            if (profileFld.Exists && profilePics != null)
            {
                var files = profileFld.GetFiles();
                result.AddRange(from fi in files
                    let file = File.ReadAllBytes(fi.FullName)
                    let props = new Hashtable
                    {
                        {"ContentType", "Picture"}, {"Title", fi.Name},
                    }
                    let folder = profilePics.RootFolder
                    let f = folder.Files.Add(fi.Name, file, props, true)
                    select new KeyValuePair<string, string>(fi.Name.Substring(0,fi.Name.LastIndexOf(".", StringComparison.Ordinal)), f.ServerRelativeUrl.RemoveBlanks()));
            }
            Console.WriteLine("Profile Picture Import Complete");

            return result.ToArray();
        }

        public static void GetFolders(ExchangeService service, List<Folder> flds)
        {
            FolderView folderView = new FolderView(int.MaxValue);
            FindFoldersResults found = service.FindFolders(WellKnownFolderName.PublicFoldersRoot, folderView);
            foreach (Folder folder in found)
            {
                if (folder.DisplayName.Contains(ReiserIntranetConfig.FldQualifier))
                {
                    flds.Add(folder);
                }
                FindAllSubFolders(service, folder.Id, flds);
            }
        }

        private static void FindAllSubFolders(ExchangeService service, FolderId parentFolderId, List<Folder> flds)
        {
            FolderView folderView = new FolderView(int.MaxValue);
            FindFoldersResults found = service.FindFolders(parentFolderId, folderView);
            foreach (Folder folder in found)
            {
                if (folder.DisplayName.Contains(ReiserIntranetConfig.FldQualifier))
                {
                    flds.Add(folder);
                }

                FindAllSubFolders(service, folder.Id, flds);
            }
        }
    }
}
