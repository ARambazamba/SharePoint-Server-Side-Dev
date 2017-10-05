using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;

namespace EventReceivers.Features.DemoEventReceivers
{
    [Guid("7ce11561-a366-4f77-8147-5e99699276c9")]
    public class DemoEventReceiversEventReceiver : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPWeb web = (SPWeb)properties.Feature.Parent;
                SPList list = web.Lists["MyList"];

                if (list.BaseTemplate == SPListTemplateType.DocumentLibrary)
                {
                    string AssemblyName = Assembly.GetExecutingAssembly().FullName;
                    string EventReceiverImplClass = "EventReceivers.AutoTitleReceiver";
                    list.EventReceivers.Add(SPEventReceiverType.ItemAdded, AssemblyName, EventReceiverImplClass);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
