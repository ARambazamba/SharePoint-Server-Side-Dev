using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace PersistingValues.Layouts.PersistingValues
{
    public partial class ValuesPage : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var ascx = Page.LoadControl("~/_ControlTemplates/15/PersistingValues/ucPersistingValues.ascx");
            pContent.Controls.Add(ascx);

        }
    }
}
