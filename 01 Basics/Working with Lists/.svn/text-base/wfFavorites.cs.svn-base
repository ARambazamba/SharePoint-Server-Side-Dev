using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.SharePoint;

namespace Integrations
{
    public partial class wfFavorites : Form
    {
        public wfFavorites()
        {
            InitializeComponent();
        }

        private SPSite Collection;
        private List<IEFavoritesEntry> FavoritesList;

        private void wfFavoritesLoad(object sender, EventArgs e)
        {
            FavoritesList = GetFavorites();

            foreach (IEFavoritesEntry item in FavoritesList)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.SubItems.Add(item.Display);
                lvFavorites.Items.Add(lvi);
            }
        }

        #region "Read Favorites"

        private static List<IEFavoritesEntry> GetFavorites()
        {
            string FavoritesPath = Environment.GetFolderPath(Environment.SpecialFolder.Favorites);
            DirectoryInfo FavoritesDir = new DirectoryInfo(FavoritesPath);

            List<IEFavoritesEntry> result = new List<IEFavoritesEntry>();
            IEFavoritesEntry item;

            foreach (FileInfo file in FavoritesDir.GetFiles("*.url", SearchOption.AllDirectories))
            {
                item = new IEFavoritesEntry();
                item.Display = file.Name.Substring(0, file.Name.Length - 4);
                item.Topic = file.Directory.Name;
                item.Url = ReadURLFromFile(file.FullName);
                result.Add(item);
            }
            return result;
        }

        private static string ReadURLFromFile(string path)
        {
            StreamReader sr = File.OpenText(path);
            string result = null;
            string line = null;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Substring(0, 3).ToLower() == "url")
                {
                    result = line.Substring(4);
                    break;
                }
            }
            return result;
        }

        #endregion

        private void SiteSelected(object sender, EventArgs e)
        {
            SPListCollection lists = ((SPWeb) cbSite.SelectedItem).Lists;

            cbList.Items.Clear();

            foreach (SPList list in lists)
            {
                cbList.Items.Add(list);
            }

            if (lists.Count > 0)
            {
                cbList.SelectedIndex = 0;
            }
        }

        private void ConnectToSite(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Collection = new SPSite(txtCollection.Text);
            cbSite.Items.Clear();

            foreach (SPWeb web in Collection.AllWebs)
            {
                cbSite.Items.Add(web);
            }

            if (Collection.AllWebs.Count > 0)
            {
                cbSite.SelectedIndex = 0;
            }
        }

        private void UploadFavorites(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SPList list = (SPList) cbList.SelectedItem;

            foreach (IEFavoritesEntry favorite in FavoritesList)
            {
                SPListItem item = list.Items.Add();
                item["Title"] = favorite.Display;

                //sp structure to store urls
                SPFieldUrlValue url = new SPFieldUrlValue();
                url.Url = favorite.Url;
                url.Description = favorite.Display;

                item["URL"] = url;
                item["Topic"] = favorite.Topic;
                item.Update();
            }

            MessageBox.Show("Favorites transfered");

            // if you want look at this .... gives you an impression about sp internal structure of fields
            IterateFieldsCollection(list.Items);
        }

        private static void IterateFieldsCollection(SPListItemCollection entries)
        {
// demo just to give you an impression of the fields collection
            if (entries.Count > 0)
            {
                foreach (SPField field in entries[0].Fields)
                {
                    object value = null;
                    string display = field.Title;

                    if (entries[0][display] == null)
                    {
                        value = "not set";
                    }
                    else
                    {
                        value = entries[0][display];
                    }
                    Debug.WriteLine(String.Format("{0} : {1}, Type {2}", display, value, field.TypeDisplayName));
                }
            }
        }
    }
}