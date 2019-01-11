using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.Threading;

namespace Stumble1
{
	public partial class Stumble1 : Form
	{
		const string BaseUrl = "https://www.stumbleupon.com/stumbler/";
		string UserID = "lrs31415";

		EdgeDriver drv;

		public Stumble1()
		{
			InitializeComponent();
		}

		private void Stumble1_Load(object sender, EventArgs e)
		{
			var wc = new WebClient();
			string Url = $"{BaseUrl}{UserID}/lists";
			var html = wc.DownloadString(Url);

			// web1.Navigated += Web1_Navigated;

		}

		private void Web1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
		{
			// TODO:
		}

		private void BtnGo_Click(object sender, EventArgs e)
		{
			// string Url = $"{BaseUrl}{UserID}/lists";
			// web1.Navigate(Url);
			drv = new EdgeDriver();
			drv.Url = $"{BaseUrl}{UserID}/lists";
			// drv.Url = "https://www.bing.com/";
			// Find the search box and query for webdriver
			// var element = drv.FindElementById("sb_form_q");
			// element.SendKeys("webdriver");
			// element.SendKeys(OpenQA.Selenium.Keys.Enter);
			Thread.Sleep(5000);	// Kludge
			string txt = drv.PageSource;
			Clipboard.SetText(txt);
			// drv.Quit();
		}
	}
}
