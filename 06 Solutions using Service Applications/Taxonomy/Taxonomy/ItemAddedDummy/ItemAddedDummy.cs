using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace Taxonomy.ItemAddedDummy
{
    public class ItemAddingDummy : SPItemEventReceiver
    {
        /// <summary>
        /// An item is being added.
        /// </summary>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            SPListItem item = properties.ListItem;

            item["Title"] = item.Title + " ... added";
        }
    }
}