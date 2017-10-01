using System;
using System.Diagnostics;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Workflow;

namespace Taxonomy.Layouts.Taxonomy
{
    public partial class ContentTypes : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Declare Fields

            var site = SPContext.Current.Site;
            var web = site.RootWeb;
            // Get the SPFieldCollection for the root web.
            var fields = web.Fields;
            // Add a new date field.
            var fieldExpiryDate = new SPFieldDateTime(fields, SPFieldType.DateTime.ToString(), "Expiry Date")
            {
                StaticName = "ExpiryDate",
                DisplayFormat = SPDateTimeFieldFormatType.DateOnly,
                Group = "Contoso Columns",
                Required = true
            };
            fieldExpiryDate.Update();
            fields.Add(fieldExpiryDate);

            // Add a new choice field.
            var fieldProductionType = new SPFieldChoice(fields, SPFieldType.Choice.ToString(), "Production Type")
            {
                StaticName = "ProductionType"
            };
            fieldProductionType.Choices.Add("Research");
            fieldProductionType.Choices.Add("Prototype");
            fieldProductionType.Choices.Add("Trial");
            fieldProductionType.Choices.Add("Release");
            fieldProductionType.Group = "Contoso Columns";
            fieldProductionType.Required = true;
            fieldProductionType.Update();
            fields.Add(fieldProductionType);

            // Declare Content Types

            // Get the ID of the parent content type.
            var parentId = SPBuiltInContentTypeId.Document;
            // Create the Invoice content type.
            var ctInvoice = new SPContentType(parentId, web.ContentTypes, "Invoice");
            // Create field links.
            var fldClientName = fields.GetField("ClientName");
            var fldPaymentDueDate = fields.GetField("PaymentDueDate");
            var fldAmount = fields.GetField("Amount");
            var fldLinkClientName = new SPFieldLink(fldClientName);
            var fldLinkPaymentDueDate = new SPFieldLink(fldPaymentDueDate);
            var fldLinkAmount = new SPFieldLink(fldAmount);
            // Add the field links to the content type.
            ctInvoice.FieldLinks.Add(fldLinkClientName);
            ctInvoice.FieldLinks.Add(fldLinkPaymentDueDate);
            ctInvoice.FieldLinks.Add(fldLinkAmount);
            // Persist the changes to the content database.
            ctInvoice.Update();
            // Add the content type to the collection.
            web.ContentTypes.Add(ctInvoice);
            web.Dispose();

            // Bind Content Type

            // Get the list.
            var list = web.GetList("Invoices");
            // Get the site content type.
            var ct = web.AvailableContentTypes["Invoice"];
            // Enable content types on the list.
            list.ContentTypesEnabled = true;
            // Add the content type to the list.
            list.ContentTypes.Add(ct);
            // Persist the changes to the database.
            list.Update();

            //Set Document Template

            ct.DocumentTemplate = "SiteAssets/MyTemplate.dotx";
            ct.Update();

            //Set Workflow Template

            SPWorkflowTemplate template = web.WorkflowTemplates.GetTemplateByName("InvoiceWorkflow",
                web.Locale);
// Create a new workflow association.
            SPWorkflowAssociation association = SPWorkflowAssociation.CreateWebContentTypeAssociation(template, "Process Invoice", "Invoice Tasks", "Process Invoice History");

            if (ct.WorkflowAssociations.GetAssociationByName("Process Invoice", web.Locale) == null)
            {
                ct.WorkflowAssociations.Add(association);
                ct.UpdateWorkflowAssociationsOnChildren(false, true, true, false);
            }
        }
    }
}
