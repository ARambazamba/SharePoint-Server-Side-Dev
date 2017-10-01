using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace EventReceivers
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class TitleChangeSyncrReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// An item was updated.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdating(properties);
            string bTitle = properties.BeforeProperties["Title"].ToString();
            string aTitle = properties.AfterProperties["Title"].ToString();

            if (bTitle != aTitle)
            {
                properties.ErrorMessage = "Title change is not allowed";
                properties.Status = SPEventReceiverStatus.CancelWithError;
                properties.Cancel = true;
            }
        }
    }
}