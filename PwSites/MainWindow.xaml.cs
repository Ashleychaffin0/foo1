using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.EntityFrameworkCore.Sqlite;
// using Microsoft.Data.Sqlite;

namespace PwSites {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		PwSitesContext ctx;

//---------------------------------------------------------------------------------------

		public MainWindow() {
			InitializeComponent();
			CmbSite.Items.Add("Hello");
			CmbSite.Items.Add("there");
			CmbSite.Items.Add("world");
			ctx = new PwSitesContext();
		}

//---------------------------------------------------------------------------------------

		private void BtnGo_Click(object sender, RoutedEventArgs e) {
			ctx.Database.EnsureCreated();
			var newsite = new Site {
				// SiteId = 1,
				Name = "Elizabeth",
				Description = "Lincoln",
				UidPwId = 23
			};

			ctx.Site.Add(newsite);
			ctx.SaveChanges();
		}
	}
}
