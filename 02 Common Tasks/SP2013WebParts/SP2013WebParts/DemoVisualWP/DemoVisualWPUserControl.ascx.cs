using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SP2013WebParts.DemoVisualWP
{
    public partial class DemoVisualWPUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ButtonClicked(object sender, EventArgs e)
        {
            lblMsg.Text = "You clicked the button";
        }
    }
}
