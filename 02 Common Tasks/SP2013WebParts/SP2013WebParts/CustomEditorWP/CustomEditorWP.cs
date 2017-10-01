using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SP2013WebParts.CustomEditorWP
{
    [ToolboxItemAttribute(false)]
    public class CustomEditorWP : WebPart
    {
        public string ListName { get; set; }

        protected DropDownList ddTitleSelection;
        
        protected override void CreateChildControls()
        {
        }

          public override EditorPartCollection CreateEditorParts()
        {
            ArrayList partsArray = new ArrayList();
            CustomEditorPart ce = new CustomEditorPart { ID = ID + "_editorPart1", Title = "List" };
            partsArray.Add(ce);
            EditorPartCollection parts = new EditorPartCollection(partsArray);
            return parts;
        }

             public void BindList()
        {
            if (ddTitleSelection != null)
            {
                ddTitleSelection.DataSource = GetFilterValues();
                ddTitleSelection.DataBind();
            }
        }

            protected List<string> GetFilterValues()
        {
            SPWeb web = SPContext.Current.Web;
            SPList list = web.Lists[ListName];
            List<string> result = null;

            if (list != null)
            {
                result = new List<string>();
                foreach (SPListItem c in list.Items)
                {
                    if (result.Contains(c.Title) == false)
                    {
                        result.Add(c.Title);
                    }
                }
            }

            return result;
        }
    }

    public class CustomEditorPart : EditorPart
    {
        protected Label lblListMsg;
        protected DropDownList ddList;
        protected Label lblMsg;

        protected override void CreateChildControls()
        {
            lblListMsg = new Label {Text = "Please choose a List to use<br>"};
            Controls.Add(lblListMsg);

            ddList = new DropDownList();
            SPWeb web = SPContext.Current.Web;
            foreach (SPList list in web.Lists)
            {
                if (list.Hidden==false)
                {
                    ddList.Items.Add(list.Title);
                }
            }
            Controls.Add(ddList);

            lblMsg = new Label();
            Controls.Add(lblMsg);
        }

        // is used when the user presses ok in the webpart config menu
        public override bool ApplyChanges()
        {
            try
            {
                if (ddList.SelectedValue != string.Empty)
                {
                    ((CustomEditorWP) WebPartToEdit).ListName = ddList.SelectedValue;
                    ((CustomEditorWP) WebPartToEdit).BindList();
                    lblMsg.Text = string.Empty;
                }
                else
                {
                    ((CustomEditorWP) WebPartToEdit).ListName = "Please select a list";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text += ex.Message;
            }
            return true;
        }

        //reads the current val from the webpart
        public override void SyncChanges()
        {
            try
            {
                ddList.Text = ((CustomEditorWP) WebPartToEdit).ListName;
            }
            catch
            {
            }
        }
    }
}

