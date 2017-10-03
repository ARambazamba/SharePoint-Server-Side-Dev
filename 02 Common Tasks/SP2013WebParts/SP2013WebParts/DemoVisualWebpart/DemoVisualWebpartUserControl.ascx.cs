using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace SP2013WebParts.DemoVisualWebpart
{
    public partial class DemoVisualWebpartUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ButtonClicked(object sender, EventArgs e)
        {
            var msg = string.Format("Hello {0}, welcome to site {1}", SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.Title);
            Label1.Text = msg;
        }
    }
}
