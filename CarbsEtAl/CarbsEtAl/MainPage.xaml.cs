using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace CarbsEtAl {
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MainPage : ContentPage {

//---------------------------------------------------------------------------------------

		public MainPage() {
			InitializeComponent();

			string PersonalUrl = GetLink();
			UserName.Text = DeviceInfo.Name;

			DateTime LastUpdated = GetLastUpdated(PersonalUrl);
			string xml = GetUrlContentsFromOneDrive(PersonalUrl);
			var xdoc = new XmlDocument();
			xdoc.LoadXml(xml);
			var Nuts = new List<Nutrition>();
			foreach (XmlElement node in xdoc.DocumentElement) {
				Nuts.Add(new Nutrition(node));
			}
			ItemList.ItemsSource = Nuts;
		}

//---------------------------------------------------------------------------------------

		private string GetLink() {
			// Check the device that's being used. If it's my phone or Barb's, allow
			// updates. Otherwise, just allow read access.
			string LinkRead    = "https://1drv.ms/u/s!AhzS1EhFU36Rpu4KOTUxKwzJUf7mow?e=8onqM5";
			string LinkEditLRS = "https://1drv.ms/u/s!AhzS1EhFU36Rpu4KOTUxKwzJUf7mow?e=MgxLgO";
			string LinkEditBga = "https://1drv.ms/u/s!AhzS1EhFU36Rpu4KOTUxKwzJUf7mow?e=VE1mNs";
			string url;
			switch (DeviceInfo.Name) {
			case "lrs8920":
			case "LRS G7":
				// Add tablet IDs for me 
				url = LinkEditLRS;
				break;
			case "bga101":
				// Add phone and tablet IDs for Barb 
				url = LinkEditBga;
				break;
			default:
				url = LinkRead;
				break;
			}
			return url;
		}

//---------------------------------------------------------------------------------------

		private DateTime GetLastUpdated(string url) {
			string Info = GetUrlContentsFromOneDrive(url, addContent: false);
			JObject obj = JObject.Parse(Info);
			var LastUpdated = DateTime.Parse(obj.Value<string>("lastModifiedDateTime"));
			return LastUpdated;
		}

//---------------------------------------------------------------------------------------

		private string GetUrlContentsFromOneDrive(string url, bool addContent = true) {
			// Have no idea what's going on here, but thanks to 
			// https://rextester.com/QNSY6325
			// via
			// https://towardsdatascience.com/how-to-get-onedrive-direct-download-link-ecb52a62fee4
			var wc = new WebClient();
			string base64Value = Convert.ToBase64String(Encoding.UTF8.GetBytes(url));
			string encodedUrl = "u!" + base64Value.TrimEnd('=').Replace('/', '_').Replace('+', '-');
			string resultUrl = $"https://api.onedrive.com/v1.0/shares/{encodedUrl}/root";
			if (addContent) { resultUrl += "/content"; }
			string text = wc.DownloadString(resultUrl);
			return text;
		}

//---------------------------------------------------------------------------------------

		private void ItemList_ItemSelected(object sender, SelectedItemChangedEventArgs _) {
			var CurItem = (Nutrition)(sender as ListView).SelectedItem;
			Unit.Text     = CurItem.Unit;
			Protein.Text  = CurItem.Protein;
			Carbs.Text    = CurItem.Carbs;
			Calories.Text = CurItem.Calories;
			Fat.Text      = CurItem.Fat;
			SatFat.Text   = CurItem.SatFat;
		}
	}
}
