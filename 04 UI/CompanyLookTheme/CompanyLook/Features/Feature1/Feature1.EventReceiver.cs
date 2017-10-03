using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;

namespace CompanyLook.Features.Feature1
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("58f1e0c9-4caf-4f7d-8ff4-5d60b600967c")]
    public class Feature1EventReceiver : SPFeatureReceiver
    {
        const string THEMENAME = "Company Look Theme";
        const string THEMETITLE = "CompanyLook";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var web = properties.Feature.Parent as SPWeb;
            var relativeWebUrl = web.ServerRelativeUrl;
            var relativeSiteUrl = web.Site.ServerRelativeUrl;

            if (!relativeWebUrl.EndsWith("/")) relativeWebUrl += "/";
            if (!relativeSiteUrl.EndsWith("/")) relativeSiteUrl += "/";

            var list = web.GetList(string.Format("{0}_catalogs/design", relativeWebUrl));

            if (list == null) return;

            SPQuery query = new SPQuery();
            query.ViewFields = "<FieldRef Name='ThemeUrl' />";
            query.RowLimit = 10;
            query.Query = string.Format("<Where><Eq><FieldRef Name='Name' /><Value Type='Text'>{0}</Value></Eq></Where>", THEMENAME);
            SPListItemCollection listItems = list.GetItems(query);

            if (listItems.Count >= 1) return;

            var item = list.AddItem();

            item["Title"] = THEMETITLE;
            item["Name"] = THEMENAME;
            item["DisplayOrder"] = 1;

            var masterPageUrl = new SPFieldUrlValue();
            masterPageUrl.Url = masterPageUrl.Description = string.Format("{0}_catalogs/masterpage/seattle.master", relativeWebUrl);
            item["MasterPageUrl"] = masterPageUrl;

            var themeUrl = new SPFieldUrlValue();
            themeUrl.Url = themeUrl.Description = string.Format("{0}_catalogs/theme/15/companylook-palette001.spcolor", relativeSiteUrl);
            item["ThemeUrl"] = themeUrl;

            var fontSchemeUrl = new SPFieldUrlValue();
            fontSchemeUrl.Url = fontSchemeUrl.Description = string.Format("{0}_catalogs/theme/15/companylook-fontscheme001.spfont", relativeSiteUrl);
            item["FontSchemeUrl"] = fontSchemeUrl;

            var imageUrl = new SPFieldUrlValue();
            imageUrl.Url = imageUrl.Description = string.Format("{0}SiteAssets/Background.png", relativeSiteUrl);
            item["ImageUrl"] = imageUrl;

            item.Update();
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            var web = properties.Feature.Parent as SPWeb;
            var relativeWebUrl = web.ServerRelativeUrl;

            if (!relativeWebUrl.EndsWith("/")) relativeWebUrl += "/";

            var list = web.GetList(string.Format("{0}_catalogs/design", relativeWebUrl));

            if (list == null) return;

            SPQuery query = new SPQuery();
            query.ViewFields = "<FieldRef Name='ThemeUrl' />";
            query.RowLimit = 10;
            query.Query = string.Format("<Where><Eq><FieldRef Name='Name' /><Value Type='Text'>{0}</Value></Eq></Where>", THEMENAME);

            SPListItemCollection listItems = list.GetItems(query);

            if (listItems.Count >= 1)
            {
                listItems.Delete(0);
            }

        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
