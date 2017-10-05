using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace EventReceivers
{
    public class TitleChangeSyncrReceiver : SPItemEventReceiver
    {
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            base.ItemUpdating(properties);
            string bTitle = properties.BeforeProperties["Title"].ToString();
            string aTitle = properties.AfterProperties["Title"].ToString();

            if (bTitle != aTitle)
            {
                properties.ErrorMessage = "Title change is not allowed";
                properties.Status = SPEventReceiverStatus.CancelWithError;
                //properties.Cancel = true;
            }
        }
    }
}