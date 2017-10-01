using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace PersistingValues.CONTROLTEMPLATES.PersistingValues
{
    public partial class ucPersistingValues : UserControl
    {
        private string listName = "MyCustomList";
        private string valueKey = "MyCustomListValue";

        protected void Page_Load(object sender, EventArgs e)
        {
            GetTextboxValue(txtUser);
            GetTextboxValue(txtPassword);
            AddListValue();
        }

        private void AddListValue()
        {
            SPList list = SPContext.Current.Web.Lists[listName];
            if (list.RootFolder.Properties.ContainsKey(valueKey))
            {
                txtListParameter.Text = list.RootFolder.Properties[valueKey].ToString();
            }
        }

        protected void GetTextboxValue(TextBox txtBox)
        {
            if (SPContext.Current.Web.Properties[txtBox.ID] != null)
            {
                txtBox.Text = SPContext.Current.Web.Properties[txtBox.ID];
            }
        }

        protected void SaveTextboxValue(object sender, EventArgs e)
        {
            SPContext.Current.Web.AllowUnsafeUpdates = true;
            SaveValueToWebPropertyBag(txtUser);
            SaveValueToWebPropertyBag(txtPassword);
            SaveCustomListValue();
            SPContext.Current.Web.AllowUnsafeUpdates = false;

        }

        private void SaveCustomListValue()
        {
            SPList list = SPContext.Current.Web.Lists[listName];
            if (list.RootFolder.Properties.ContainsKey(valueKey))
            {
                list.RootFolder.Properties.Remove(valueKey);
            }
            list.RootFolder.Properties.Add(valueKey, txtListParameter.Text);
            list.RootFolder.Update();
            list.Update();
        }

        protected void SaveValueToWebPropertyBag(TextBox txtBox)
        {
            if (SPContext.Current.Web.Properties.ContainsKey(txtBox.ID))
            {
                SPContext.Current.Web.Properties.Remove(txtBox.ID);
            }

            SPContext.Current.Web.Properties.Add(txtBox.ID, txtBox.Text);
            SPContext.Current.Web.Properties.Update();
            SPContext.Current.Web.Update();
        }
    }
}
