using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace SiteArchiveFeature.SalesWebDeletingReceiver
{
    public class SalesWebDeletingReceiver : SPWebEventReceiver
    {
        public override void WebDeleting(SPWebEventProperties properties)
        {
            using (var site = new SPSite(properties.SiteId))
            {
                string backupLocation = @"E:\Labfiles\Starter\Backup.backup";
                site.WebApplication.Sites.Backup(site.Url, backupLocation, true);
            }
        }
    }
}