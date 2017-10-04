using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

namespace BasicFileOperations
{
    class Program
    {
        static void Main(string[] args)
        {

            SPSite site = new SPSite("http://sp2016");
            SPWeb web = site.AllWebs["Sales"];
            SPList list = web.Lists["Orders"];
                        
            //Export a file
            if (list.BaseTemplate == SPListTemplateType.DocumentLibrary && list.ItemCount > 0)
            {                         
                SPListItem item = list.Items[0];
                Debug.WriteLine(string.Format("Exporting: {0}", item.Name));
                Stream input = item.File.OpenBinaryStream();
                Stream target = File.Create(string.Format(@"c:\{0}", item.Name));

                BinaryReader reader = new BinaryReader(input);
                BinaryWriter writer = new BinaryWriter(target);
                writer.Write(reader.ReadBytes((int)input.Length));
                writer.Flush();
            }

            //Import a file - Hashtable holds Metadata
            const string path = @"c:\myword.docx";
            byte[] file = File.ReadAllBytes(path);

            Hashtable props = new Hashtable
                {
                    {"ContentType", "Document"},
                    {"Title", "My super demo word file"}
                };
            
            SPFolder folder = list.RootFolder;
            SPFile f = folder.Files.Add(path.Substring(3), file, props, true);

            //Change the checkout state
            if (f.CheckOutType == SPFile.SPCheckOutType.None)
            {
                //Check out to local drafts folder
                f.CheckOut(SPFile.SPCheckOutType.Offline, DateTime.Now.ToString());
            }
            else
            {
                //Check in a minor version
                f.CheckIn("Checking in a minor version", SPCheckinType.MinorCheckIn);
            }

            // Accessing File Versions
            foreach (SPFileVersion v in f.Versions)
            {
                Debug.WriteLine("Version: " + v.VersionLabel);
                SPFile fv = v.File;
                // If you want to dump this version repeat the pattern used above
            }

         }
    }
}
