using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace EventReceivers
{
    public class AutoTitleReceiver : SPItemEventReceiver
    {
        public override void ItemAdded(SPItemEventProperties properties)
        {            
            SPListItem item = properties.ListItem;
            if (item.Title == string.Empty)
            {
                item["Title"] = item.Name.Substring(0, item.Name.LastIndexOf("."));
            }

            item.Update();
        }
    }
}