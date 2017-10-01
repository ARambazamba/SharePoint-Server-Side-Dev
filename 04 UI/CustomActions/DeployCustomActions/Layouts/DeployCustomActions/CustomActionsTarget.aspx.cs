using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace DeployCustomActions.Layouts.DeployCustomActions
{
    public partial class CustomActionsTarget : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ItemID"] != null && Request.QueryString["ListID"] != null)
            {
                var itemid = Int32.Parse(Request.QueryString["ItemID"]);
                var listid = new Guid(Request.QueryString["ListID"]);

                SPWeb web = SPContext.Current.Web;
                SPList list = web.Lists[listid];
                SPListItem item = list.Items[itemid];
            }
        }
    }
}
