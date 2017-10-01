using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SP2013WebParts.SimpleWebPart
{
    [ToolboxItemAttribute(false)]
    public class SimpleWebPart : WebPart
    {
        private Label lblMessage;
        private Button btnAction;

        protected override void CreateChildControls()
        {
            lblMessage = new Label { Text = "Initilized by load" };
            Controls.Add(lblMessage);

            btnAction = new Button { Text = "Click me", ID = "btnAction" };
            btnAction.Click += btnAction_Click;
            Controls.Add(btnAction);
        }

        void btnAction_Click(object sender, EventArgs e)
        {
            string msg = string.Format("Hallo {0} - thank you for clicking", SPContext.Current.Web.CurrentUser.LoginName);
            lblMessage.Text = msg;
        }
    }
}
